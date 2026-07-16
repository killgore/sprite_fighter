import SpriteKit

enum GameState {
    case title
    case playing
    case levelComplete
    case store
    case gameOver
}

private enum StoreItem {
    case repairs
    case weapon(WeaponType)

    var cost: Int {
        switch self {
        case .repairs: return 100
        case .weapon(let type): return type.cost
        }
    }

    var name: String {
        switch self {
        case .repairs: return "Repairs (+100)"
        case .weapon(let type): return type.displayName
        }
    }
}

final class GameScene: SKScene {
    private var state: GameState = .title

    private var player: PlayerShip!
    private var enemies: [EnemyShip] = []
    private var playerProjectiles: [Projectile] = []
    private var enemyProjectiles: [Projectile] = []
    private var creditOrbs: [CreditOrb] = []
    private var starField: StarField!

    private var lastUpdateTime: TimeInterval = 0
    private var currentTimeCache: TimeInterval = 0
    private var enemySpawnAccumulator: TimeInterval = 0
    private let enemySpawnCheckInterval: TimeInterval = 1.0 / 30.0

    private var touchLocation: CGPoint?
    private var isTouching = false

    private var score = 0
    private var highScore = 0

    private var currentLevel = 1
    private var killsThisLevel = 0

    private var gameOverTime: TimeInterval = 0
    private let gameOverWait: TimeInterval = 3.0

    private var scoreLabel: SKLabelNode!
    private var lifeLabel: SKLabelNode!
    private var highScoreLabel: SKLabelNode!
    private var creditsLabel: SKLabelNode!
    private var levelLabel: SKLabelNode!

    private var overlayNode: SKNode?
    private var menuButtons: [(node: SKLabelNode, action: () -> Void)] = []

    private var selectedStoreItem: StoreItem?
    private var storeItemLabels: [(item: StoreItem, label: SKLabelNode)] = []
    private var storeMessageLabel: SKLabelNode?
    private var storeCreditsLabel: SKLabelNode?
    private var storeLifeLabel: SKLabelNode?

    private let playerTexture = SKTexture(imageNamed: "PlayerShip")
    private let gruntTexture = SKTexture(imageNamed: "GruntShip")
    private let smileyTexture = SKTexture(imageNamed: "SmileyShip")
    private let juggernautTexture = SKTexture(imageNamed: "JuggernautShip")
    private let shieldTexture = SKTexture(imageNamed: "Shield")
    private let projectileTexture = SKTexture(imageNamed: "Projectile")
    private let explosionTexture = SKTexture(imageNamed: "Explosion")
    private let starTexture = SKTexture(imageNamed: "Star")

    private static let highScoreKey = "highScore"

    private let storeItems: [StoreItem] = [
        .repairs,
        .weapon(.ionCannon),
        .weapon(.spreadCannon),
        .weapon(.gravityWell),
        .weapon(.shield),
        .weapon(.stasisRay)
    ]

    override func didMove(to view: SKView) {
        backgroundColor = .black
        highScore = UserDefaults.standard.integer(forKey: GameScene.highScoreKey)

        starField = StarField(parent: self, texture: starTexture, bounds: CGRect(origin: .zero, size: size))

        let music = SKAudioNode(fileNamed: "Spritefighter.wav")
        music.autoplayLooped = true
        addChild(music)

        player = PlayerShip(texture: playerTexture)
        player.position = CGPoint(x: size.width / 2, y: size.height * 0.15)
        player.zPosition = 10
        player.isHidden = true
        addChild(player)

        setupHUD()
        showTitleScreen()
    }

    // MARK: - HUD

    private func setupHUD() {
        levelLabel = makeLabel(text: "Level: 1", position: CGPoint(x: 16, y: size.height - 26), fontSize: 14, color: .white)
        levelLabel.horizontalAlignmentMode = .left

        lifeLabel = makeLabel(text: "Life: 1000", position: CGPoint(x: 16, y: size.height - 60), fontSize: 16, color: .green)
        lifeLabel.horizontalAlignmentMode = .left

        scoreLabel = makeLabel(text: "Score: 0", position: CGPoint(x: size.width - 16, y: size.height - 60), fontSize: 16, color: .white)
        scoreLabel.horizontalAlignmentMode = .right

        highScoreLabel = makeLabel(text: "High Score: 0", position: CGPoint(x: size.width / 2, y: size.height - 90), fontSize: 14, color: .cyan)

        creditsLabel = makeLabel(text: "Credits: 0", position: CGPoint(x: size.width / 2, y: size.height - 116), fontSize: 14, color: UIColor(red: 0.18, green: 0.55, blue: 0.34, alpha: 1))

        for label in [levelLabel, scoreLabel, lifeLabel, highScoreLabel, creditsLabel] {
            label?.zPosition = 20
            label?.isHidden = true
            addChild(label!)
        }
    }

    private func makeLabel(text: String, position: CGPoint, fontSize: CGFloat, color: UIColor) -> SKLabelNode {
        let label = SKLabelNode(fontNamed: "AvenirNext-Bold")
        label.text = text
        label.fontSize = fontSize
        label.fontColor = color
        label.position = position
        label.horizontalAlignmentMode = .center
        return label
    }

    private func updateHUD() {
        levelLabel.text = "Level: \(currentLevel)"
        scoreLabel.text = "Score: \(score)"
        lifeLabel.text = "Life: \(player.life)"
        highScoreLabel.text = "High Score: \(highScore)"
        creditsLabel.text = "Credits: \(player.credits)"

        let ratio = CGFloat(player.life) / CGFloat(PlayerShip.maxLife)
        lifeLabel.fontColor = ratio < 0.25 ? .red : (ratio < 0.5 ? .yellow : .green)
    }

    private func setHUDHidden(_ hidden: Bool) {
        for label in [levelLabel, scoreLabel, lifeLabel, highScoreLabel, creditsLabel] {
            label?.isHidden = hidden
        }
    }

    // MARK: - Screen management

    private func clearOverlay() {
        overlayNode?.removeFromParent()
        overlayNode = nil
        menuButtons.removeAll()
        storeItemLabels.removeAll()
        storeMessageLabel = nil
        storeCreditsLabel = nil
        storeLifeLabel = nil
        selectedStoreItem = nil
    }

    private func addButton(text: String, position: CGPoint, fontSize: CGFloat, color: UIColor, to parent: SKNode, action: @escaping () -> Void) -> SKLabelNode {
        let label = makeLabel(text: text, position: position, fontSize: fontSize, color: color)
        parent.addChild(label)
        menuButtons.append((label, action))
        return label
    }

    private func showTitleScreen() {
        clearOverlay()
        state = .title
        player.isHidden = true
        setHUDHidden(true)

        let node = SKNode()
        node.zPosition = 30
        let title = makeLabel(text: "SPRITE FIGHTER", position: CGPoint(x: size.width / 2, y: size.height * 0.6), fontSize: 30, color: .white)
        let instruct = makeLabel(text: "Touch Screen To Play!", position: CGPoint(x: size.width / 2, y: size.height * 0.5), fontSize: 18, color: .white)
        let hs = makeLabel(text: "High Score: \(highScore)", position: CGPoint(x: size.width / 2, y: size.height * 0.44), fontSize: 16, color: .cyan)
        node.addChild(title)
        node.addChild(instruct)
        node.addChild(hs)
        addChild(node)
        overlayNode = node
    }

    private func startGame() {
        clearOverlay()

        enemies.forEach { $0.onRemoved(); $0.removeFromParent() }
        enemies.removeAll()
        playerProjectiles.forEach { $0.removeFromParent() }
        playerProjectiles.removeAll()
        enemyProjectiles.forEach { $0.removeFromParent() }
        enemyProjectiles.removeAll()
        creditOrbs.forEach { $0.removeFromParent() }
        creditOrbs.removeAll()
        JuggernautShip.reset()

        score = 0
        currentLevel = 1
        killsThisLevel = 0
        player.reset()
        player.position = CGPoint(x: size.width / 2, y: size.height * 0.15)
        player.isHidden = false

        setHUDHidden(false)
        updateHUD()

        state = .playing
    }

    private func endGame(at time: TimeInterval) {
        clearOverlay()
        state = .gameOver
        gameOverTime = time
        player.isHidden = true

        if score > highScore {
            highScore = score
            UserDefaults.standard.set(highScore, forKey: GameScene.highScoreKey)
        }

        let node = SKNode()
        node.zPosition = 30
        let title = makeLabel(text: "GAME OVER", position: CGPoint(x: size.width / 2, y: size.height * 0.55), fontSize: 30, color: .red)
        let instruct = makeLabel(text: "Touch Screen To Play Again", position: CGPoint(x: size.width / 2, y: size.height * 0.45), fontSize: 16, color: .white)
        node.addChild(title)
        node.addChild(instruct)
        addChild(node)
        overlayNode = node
    }

    private func showLevelCompleteScreen() {
        clearOverlay()
        state = .levelComplete

        let node = SKNode()
        node.zPosition = 30
        let title = makeLabel(text: "LEVEL COMPLETE!", position: CGPoint(x: size.width / 2, y: size.height * 0.62), fontSize: 26, color: .white)
        node.addChild(title)

        _ = addButton(text: "Next Level >>", position: CGPoint(x: size.width / 2, y: size.height * 0.48), fontSize: 20, color: .cyan, to: node) { [weak self] in
            self?.startNextLevel()
        }
        _ = addButton(text: "Ship Upgrades >>", position: CGPoint(x: size.width / 2, y: size.height * 0.4), fontSize: 20, color: .green, to: node) { [weak self] in
            self?.showStoreScreen()
        }

        addChild(node)
        overlayNode = node
    }

    private func showStoreScreen() {
        clearOverlay()
        state = .store
        setHUDHidden(true)

        let node = SKNode()
        node.zPosition = 30

        node.addChild(makeLabel(text: "SUPERNOVA HARDWARE", position: CGPoint(x: size.width / 2, y: size.height * 0.9), fontSize: 18, color: .orange))
        node.addChild(makeLabel(text: "Upgrade Your Guns!", position: CGPoint(x: size.width / 2, y: size.height * 0.855), fontSize: 13, color: .orange))

        let credits = makeLabel(text: "Credits: \(player.credits)", position: CGPoint(x: size.width / 2, y: size.height * 0.8), fontSize: 15, color: .green)
        node.addChild(credits)
        storeCreditsLabel = credits

        let life = makeLabel(text: "Life: \(player.life)", position: CGPoint(x: size.width / 2, y: size.height * 0.765), fontSize: 15, color: .green)
        node.addChild(life)
        storeLifeLabel = life

        let startY = size.height * 0.66
        let spacing = size.height * 0.065
        for (i, item) in storeItems.enumerated() {
            let y = startY - CGFloat(i) * spacing
            let label = makeLabel(text: "\(item.name) -- \(item.cost)", position: CGPoint(x: size.width / 2, y: y), fontSize: 15, color: .white)
            node.addChild(label)
            storeItemLabels.append((item, label))
            menuButtons.append((label, { [weak self] in self?.selectStoreItem(item) }))
        }

        let message = makeLabel(text: "", position: CGPoint(x: size.width / 2, y: size.height * 0.2), fontSize: 14, color: .red)
        node.addChild(message)
        storeMessageLabel = message

        _ = addButton(text: "$ Buy Hardware $", position: CGPoint(x: size.width / 2, y: size.height * 0.13), fontSize: 18, color: .green, to: node) { [weak self] in
            self?.purchaseSelectedItem()
        }
        _ = addButton(text: "Play >>", position: CGPoint(x: size.width / 2, y: size.height * 0.06), fontSize: 18, color: .cyan, to: node) { [weak self] in
            self?.startNextLevel()
        }

        addChild(node)
        overlayNode = node
    }

    private func selectStoreItem(_ item: StoreItem) {
        selectedStoreItem = item
        storeMessageLabel?.text = ""
        for (candidate, label) in storeItemLabels {
            label.fontColor = isSame(candidate, item) ? .green : .white
        }
        if player.credits < item.cost {
            storeMessageLabel?.text = "Insufficient Credits!"
            storeMessageLabel?.fontColor = .red
        }
    }

    private func isSame(_ a: StoreItem, _ b: StoreItem) -> Bool {
        switch (a, b) {
        case (.repairs, .repairs): return true
        case (.weapon(let t1), .weapon(let t2)): return t1 == t2
        default: return false
        }
    }

    private func purchaseSelectedItem() {
        guard let item = selectedStoreItem else { return }
        guard player.credits >= item.cost else {
            storeMessageLabel?.text = "Insufficient Credits!"
            storeMessageLabel?.fontColor = .red
            return
        }

        player.credits -= item.cost
        switch item {
        case .repairs:
            player.life += 100
        case .weapon(.shield):
            player.shield?.removeFromParent()
            let shield = ShieldComponent(texture: shieldTexture)
            shield.zPosition = 9
            addChild(shield)
            player.shield = shield
        case .weapon(let type):
            if let weapon = makeWeapon(type) {
                player.weapons.append(weapon)
            }
        }

        storeMessageLabel?.text = "Hardware Purchased!"
        storeMessageLabel?.fontColor = .green
        storeCreditsLabel?.text = "Credits: \(player.credits)"
        storeLifeLabel?.text = "Life: \(player.life)"
        selectedStoreItem = nil
        for (_, label) in storeItemLabels {
            label.fontColor = .white
        }
    }

    private func startNextLevel() {
        clearOverlay()

        if currentLevel < Levels.maxLevel {
            currentLevel += 1
        }
        killsThisLevel = 0

        enemies.forEach { $0.onRemoved(); $0.removeFromParent() }
        enemies.removeAll()
        playerProjectiles.forEach { $0.removeFromParent() }
        playerProjectiles.removeAll()
        enemyProjectiles.forEach { $0.removeFromParent() }
        enemyProjectiles.removeAll()
        creditOrbs.forEach { $0.removeFromParent() }
        creditOrbs.removeAll()
        JuggernautShip.reset()

        player.position = CGPoint(x: size.width / 2, y: size.height * 0.15)
        setHUDHidden(false)
        updateHUD()

        state = .playing
    }

    // MARK: - Touch handling

    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        guard let touch = touches.first else { return }
        let loc = touch.location(in: self)

        switch state {
        case .title:
            startGame()
        case .playing:
            touchLocation = loc
            isTouching = true
        case .gameOver:
            if (currentTimeCache - gameOverTime) > gameOverWait {
                showTitleScreen()
            }
        case .levelComplete, .store:
            handleMenuTap(at: loc)
        }
    }

    override func touchesMoved(_ touches: Set<UITouch>, with event: UIEvent?) {
        guard state == .playing, let touch = touches.first else { return }
        touchLocation = touch.location(in: self)
    }

    override func touchesEnded(_ touches: Set<UITouch>, with event: UIEvent?) {
        isTouching = false
        touchLocation = nil
    }

    override func touchesCancelled(_ touches: Set<UITouch>, with event: UIEvent?) {
        isTouching = false
        touchLocation = nil
    }

    private func handleMenuTap(at point: CGPoint) {
        for (node, action) in menuButtons {
            if node.frame.insetBy(dx: -16, dy: -14).contains(point) {
                action()
                return
            }
        }
    }

    // MARK: - Update loop

    override func update(_ currentTime: TimeInterval) {
        currentTimeCache = currentTime
        if lastUpdateTime == 0 { lastUpdateTime = currentTime }
        let dt = min(currentTime - lastUpdateTime, 1.0 / 20.0)
        lastUpdateTime = currentTime

        starField.update(dt: dt)

        if state == .playing {
            updateGameplay(dt: dt, currentTime: currentTime)
        }
    }

    private func updateGameplay(dt: TimeInterval, currentTime: TimeInterval) {
        handlePlayerMovementAndFiring(currentTime: currentTime)

        player.shield?.position = player.position

        for enemy in enemies {
            enemy.update(dt: dt, currentTime: currentTime)
        }
        handleStrikerFire(currentTime: currentTime)

        for jug in enemies.compactMap({ $0 as? JuggernautShip }) where jug.isExpired(at: currentTime) {
            jug.life = 0
        }

        playerProjectiles.forEach { $0.step(dt: dt) }
        for p in playerProjectiles where p.isExpired(at: currentTime) || !isRoughlyOnScreen(p.position) {
            p.removeFromParent()
        }
        playerProjectiles.removeAll { $0.parent == nil }

        enemyProjectiles.forEach { $0.step(dt: dt) }
        for p in enemyProjectiles where p.isExpired(at: currentTime) || !isRoughlyOnScreen(p.position) {
            p.removeFromParent()
        }
        enemyProjectiles.removeAll { $0.parent == nil }

        creditOrbs.forEach { $0.step(dt: dt) }
        for orb in creditOrbs where orb.isExpired(at: currentTime) {
            orb.removeFromParent()
        }
        creditOrbs.removeAll { $0.parent == nil }

        for e in enemies where e.position.y < -100 || e.position.y > size.height + 150 {
            e.onRemoved()
            e.removeFromParent()
        }
        enemies.removeAll { $0.parent == nil }

        enemySpawnAccumulator += dt
        while enemySpawnAccumulator >= enemySpawnCheckInterval {
            enemySpawnAccumulator -= enemySpawnCheckInterval
            trySpawnEnemy(currentTime: currentTime)
        }

        checkCollisions(dt: dt, currentTime: currentTime)

        enemies.removeAll { enemy in
            if enemy.isDead {
                onEnemyKilled(enemy, currentTime: currentTime)
                enemy.onRemoved()
                enemy.removeFromParent()
                return true
            }
            return false
        }

        if player.isDead {
            endGame(at: currentTime)
            return
        }

        if killsThisLevel >= Levels.config(for: currentLevel).killTarget {
            showLevelCompleteScreen()
            return
        }

        updateHUD()
    }

    private func isRoughlyOnScreen(_ point: CGPoint) -> Bool {
        point.x > -150 && point.x < size.width + 150 && point.y > -150 && point.y < size.height + 150
    }

    private func handlePlayerMovementAndFiring(currentTime: TimeInterval) {
        guard isTouching, let loc = touchLocation else { return }

        let halfW = player.size.width / 2
        let halfH = player.size.height / 2
        let clampedX = min(max(loc.x, halfW), size.width - halfW)
        let clampedY = min(max(loc.y, halfH), size.height * 0.75)
        player.position = CGPoint(x: clampedX, y: clampedY)

        let muzzle = CGPoint(x: player.position.x, y: player.position.y + player.size.height / 2)
        for weapon in player.weapons where weapon.canFire(currentTime: currentTime) {
            let newProjectiles = weapon.fire(from: muzzle, texture: projectileTexture, currentTime: currentTime)
            for p in newProjectiles {
                p.zPosition = 5
                addChild(p)
                playerProjectiles.append(p)
            }
            if let sound = weapon.soundFile {
                run(SKAction.playSoundFileNamed(sound, waitForCompletion: false))
            }
        }
    }

    private func handleStrikerFire(currentTime: TimeInterval) {
        for case let striker as StrikerShip in enemies where striker.canFire(currentTime: currentTime) {
            striker.lastFireTime = currentTime
            let proj = Projectile(texture: projectileTexture, position: striker.position, velocity: CGVector(dx: 0, dy: -280), damage: StrikerShip.fireDamage, spawnTime: currentTime, lifetime: 4.0)
            proj.setScale(0.2)
            proj.color = .orange
            proj.colorBlendFactor = 0.7
            proj.zPosition = 5
            addChild(proj)
            enemyProjectiles.append(proj)
        }
    }

    private func trySpawnEnemy(currentTime: TimeInterval) {
        guard let kind = Levels.config(for: currentLevel).rollSpawn() else { return }

        let margin: CGFloat = 40
        let x = CGFloat.random(in: margin...(size.width - margin))

        switch kind {
        case .grunt:
            addEnemy(GruntShip(texture: gruntTexture, position: CGPoint(x: x, y: size.height + 40)))
        case .sidewinder:
            addEnemy(SidewinderShip(texture: smileyTexture, position: CGPoint(x: x, y: size.height + 40)))
        case .striker:
            addEnemy(StrikerShip(texture: smileyTexture, position: CGPoint(x: x, y: size.height + 40)))
        case .juggernaut:
            guard JuggernautShip.canSpawn() else { return }
            let spawnPos = CGPoint(x: x, y: CGFloat.random(in: size.height * 0.6...size.height * 0.85))
            let bounds = CGRect(origin: .zero, size: size)
            addEnemy(JuggernautShip(texture: juggernautTexture, position: spawnPos, currentTime: currentTime, bounds: bounds))
        }
    }

    private func addEnemy(_ enemy: EnemyShip) {
        enemy.zPosition = 8
        addChild(enemy)
        enemies.append(enemy)
    }

    private func onEnemyKilled(_ enemy: EnemyShip, currentTime: TimeInterval) {
        spawnExplosion(at: enemy.position)
        spawnCreditOrb(at: enemy.position, currentTime: currentTime)
        score += 1
        killsThisLevel += 1
        if score > highScore { highScore = score }
    }

    private func spawnExplosion(at position: CGPoint) {
        let explosion = SKSpriteNode(texture: explosionTexture)
        explosion.position = position
        explosion.zPosition = 9
        explosion.setScale(0.5)
        addChild(explosion)
        explosion.run(SKAction.sequence([
            SKAction.fadeOut(withDuration: 0.4),
            SKAction.removeFromParent()
        ]))
        run(SKAction.playSoundFileNamed("Explosion.wav", waitForCompletion: false))
    }

    private func spawnCreditOrb(at position: CGPoint, currentTime: TimeInterval) {
        let orb = CreditOrb(texture: projectileTexture, position: position, currentTime: currentTime)
        orb.zPosition = 6
        addChild(orb)
        creditOrbs.append(orb)
    }

    private func shrink(_ rect: CGRect, factor: CGFloat) -> CGRect {
        let w = rect.width * factor
        let h = rect.height * factor
        return CGRect(x: rect.midX - w / 2, y: rect.midY - h / 2, width: w, height: h)
    }

    private func checkCollisions(dt: TimeInterval, currentTime: TimeInterval) {
        // Player projectiles vs enemy ships
        for enemy in enemies where !enemy.isDead {
            for proj in playerProjectiles where proj.parent != nil {
                let projBox = shrink(proj.frame, factor: 0.5)
                guard projBox.intersects(enemy.frame) else { continue }

                enemy.takeDamage(proj.damage)
                if proj.effect == .freeze {
                    enemy.velocity = .zero
                }
                if !proj.isPersistent {
                    proj.removeFromParent()
                    break
                }
            }
        }
        playerProjectiles.removeAll { $0.parent == nil }

        // Gravity well: persistent bolts pull every enemy toward themselves each frame
        for proj in playerProjectiles where proj.isPersistent {
            for enemy in enemies {
                let dx = proj.position.x - enemy.position.x
                let dy = proj.position.y - enemy.position.y
                let distance = max(hypot(dx, dy), 1)
                let pull = (proj.pullStrength / distance) * CGFloat(dt)
                enemy.position.x += (dx / distance) * pull
                enemy.position.y += (dy / distance) * pull
            }
        }

        guard !player.isHidden else { return }
        let playerBox = shrink(player.frame, factor: 0.5)

        // Enemy ships vs player (contact)
        for enemy in enemies where !enemy.isDead {
            guard playerBox.intersects(shrink(enemy.frame, factor: 0.5)) else { continue }
            if let shield = player.shield, !shield.isDestroyed, shieldBlocks(enemy.frame) {
                shield.absorbHit()
            } else {
                player.takeDamage(enemy.life)
            }
            enemy.life = 0
        }

        // Enemy projectiles vs player
        for proj in enemyProjectiles where proj.parent != nil {
            guard shrink(proj.frame, factor: 0.5).intersects(playerBox) else { continue }
            if let shield = player.shield, !shield.isDestroyed, shieldBlocks(proj.frame) {
                shield.absorbHit()
            } else {
                player.takeDamage(proj.damage)
            }
            proj.removeFromParent()
        }
        enemyProjectiles.removeAll { $0.parent == nil }

        // Player vs credit orbs
        for orb in creditOrbs where orb.parent != nil {
            guard playerBox.intersects(orb.frame) else { continue }
            player.credits += CreditOrb.creditValue
            orb.removeFromParent()
        }
        creditOrbs.removeAll { $0.parent == nil }
    }

    private func shieldBlocks(_ rect: CGRect) -> Bool {
        guard let shield = player.shield else { return false }
        return shield.frame.intersects(rect)
    }
}
