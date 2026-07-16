import SpriteKit

class EnemyShip: SKSpriteNode {
    var velocity: CGVector
    var life: Int
    let maxLife: Int

    init(texture: SKTexture, position: CGPoint, life: Int, velocity: CGVector) {
        self.velocity = velocity
        self.life = life
        self.maxLife = life
        super.init(texture: texture, color: .clear, size: texture.size())
        self.position = position
    }

    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }

    func step(dt: TimeInterval) {
        position.x += velocity.dx * CGFloat(dt)
        position.y += velocity.dy * CGFloat(dt)
    }

    /// Default movement just steps by velocity; ships with time-dependent motion override this.
    func update(dt: TimeInterval, currentTime: TimeInterval) {
        step(dt: dt)
    }

    func takeDamage(_ dmg: Int) {
        life = max(0, life - dmg)
    }

    var isDead: Bool { life <= 0 }

    /// Called when this ship is removed from the scene (killed, expired, or left bounds).
    func onRemoved() {}
}

final class GruntShip: EnemyShip {
    static let startingLife = 100
    static let fallSpeed: CGFloat = -90 // points/sec, downward

    init(texture: SKTexture, position: CGPoint) {
        super.init(texture: texture, position: position, life: GruntShip.startingLife, velocity: CGVector(dx: 0, dy: GruntShip.fallSpeed))
        setScale(0.5)
    }

    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
}

/// Weaves side to side on a sine wave while falling. Reuses the smiley-face sprite from the
/// original game, tinted magenta.
final class SidewinderShip: EnemyShip {
    static let startingLife = 200
    static let amplitude: CGFloat = -300 // points/sec
    static let fallSpeed: CGFloat = -150 // points/sec, downward

    private let noise: Double

    init(texture: SKTexture, position: CGPoint) {
        noise = Double.random(in: 0...1000)
        super.init(texture: texture, position: position, life: SidewinderShip.startingLife, velocity: CGVector(dx: 0, dy: SidewinderShip.fallSpeed))
        setScale(0.5)
        color = .magenta
        colorBlendFactor = 0.7
    }

    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }

    override func update(dt: TimeInterval, currentTime: TimeInterval) {
        let phase = (currentTime * 1000 + noise) / 100
        velocity.dx = CGFloat(sin(phase)) * SidewinderShip.amplitude
        step(dt: dt)
    }
}

/// Moves diagonally and fires a downward shot at the player. Reuses the smiley-face sprite,
/// tinted blue.
final class StrikerShip: EnemyShip {
    static let startingLife = 100
    static let fireRate: TimeInterval = 1.0
    static let fireDamage = 50

    var lastFireTime: TimeInterval = 0

    init(texture: SKTexture, position: CGPoint) {
        let dx: CGFloat = Bool.random() ? 90 : -90
        super.init(texture: texture, position: position, life: StrikerShip.startingLife, velocity: CGVector(dx: dx, dy: -180))
        setScale(0.5)
        color = .blue
        colorBlendFactor = 0.7
    }

    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }

    func canFire(currentTime: TimeInterval) -> Bool {
        (currentTime - lastFireTime) > StrikerShip.fireRate
    }
}

/// A tough, animated tank that bounces around the play area instead of falling, and
/// self-destructs after its lifespan expires. Only one may be active at a time.
final class JuggernautShip: EnemyShip {
    static let startingLife = 500
    static let lifespan: TimeInterval = 10.0
    static let frameSize = CGSize(width: 75, height: 75)
    static let gridColumns = 6
    static let gridRows = 8

    private static var activeInstance: JuggernautShip?

    static func canSpawn() -> Bool { activeInstance == nil }

    static func reset() {
        activeInstance = nil
    }

    let spawnTime: TimeInterval
    private let playBounds: CGRect

    init(texture: SKTexture, position: CGPoint, currentTime: TimeInterval, bounds: CGRect) {
        spawnTime = currentTime
        playBounds = bounds
        super.init(texture: texture, position: position, life: JuggernautShip.startingLife, velocity: CGVector(dx: 150, dy: -150))
        size = JuggernautShip.frameSize
        JuggernautShip.activeInstance = self
        runSpinAnimation(baseTexture: texture)
    }

    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }

    private func runSpinAnimation(baseTexture: SKTexture) {
        var frames: [SKTexture] = []
        for row in 0..<JuggernautShip.gridRows {
            for col in 0..<JuggernautShip.gridColumns {
                let rect = CGRect(x: CGFloat(col) / CGFloat(JuggernautShip.gridColumns),
                                   y: 1.0 - CGFloat(row + 1) / CGFloat(JuggernautShip.gridRows),
                                   width: 1.0 / CGFloat(JuggernautShip.gridColumns),
                                   height: 1.0 / CGFloat(JuggernautShip.gridRows))
                frames.append(SKTexture(rect: rect, in: baseTexture))
            }
        }
        run(SKAction.repeatForever(SKAction.animate(with: frames, timePerFrame: 0.05)))
    }

    func isExpired(at currentTime: TimeInterval) -> Bool {
        (currentTime - spawnTime) > JuggernautShip.lifespan
    }

    override func step(dt: TimeInterval) {
        position.x += velocity.dx * CGFloat(dt)
        position.y += velocity.dy * CGFloat(dt)

        let halfW = size.width / 2
        let halfH = size.height / 2

        if position.x > playBounds.maxX - halfW, velocity.dx > 0 { velocity.dx *= -1 }
        if position.x < playBounds.minX + halfW, velocity.dx < 0 { velocity.dx *= -1 }
        if position.y > playBounds.maxY - halfH, velocity.dy > 0 { velocity.dy *= -1 }
        if position.y < playBounds.minY + halfH, velocity.dy < 0 { velocity.dy *= -1 }
    }

    override func onRemoved() {
        if JuggernautShip.activeInstance === self {
            JuggernautShip.activeInstance = nil
        }
    }
}
