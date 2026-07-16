import SpriteKit

enum WeaponType: CaseIterable {
    case baseCannon
    case spreadCannon
    case ionCannon
    case gravityWell
    case shield
    case stasisRay

    var displayName: String {
        switch self {
        case .baseCannon: return "Base Cannon"
        case .spreadCannon: return "Spread Cannon"
        case .ionCannon: return "Ion Cannon"
        case .gravityWell: return "Gravity Well"
        case .shield: return "Shield"
        case .stasisRay: return "Stasis Ray"
        }
    }

    var cost: Int { 100 }
}

class Weapon {
    let type: WeaponType
    let fireRate: TimeInterval
    let damage: Int
    let color: UIColor
    let soundFile: String?
    let projectileScale: CGVector
    var lastFireTime: TimeInterval = 0

    init(type: WeaponType, fireRate: TimeInterval, damage: Int, color: UIColor, soundFile: String?, projectileScale: CGVector = CGVector(dx: 0.2, dy: 0.2)) {
        self.type = type
        self.fireRate = fireRate
        self.damage = damage
        self.color = color
        self.soundFile = soundFile
        self.projectileScale = projectileScale
    }

    func canFire(currentTime: TimeInterval) -> Bool {
        (currentTime - lastFireTime) > fireRate
    }

    /// Returns newly spawned projectiles. Subclasses override to define the firing pattern.
    func fire(from position: CGPoint, texture: SKTexture, currentTime: TimeInterval) -> [Projectile] {
        lastFireTime = currentTime
        return []
    }

    func makeProjectile(texture: SKTexture, position: CGPoint, velocity: CGVector, currentTime: TimeInterval, lifetime: TimeInterval = 3.0, effect: ProjectileEffect = .none, isPersistent: Bool = false, pullStrength: CGFloat = 0) -> Projectile {
        let p = Projectile(texture: texture, position: position, velocity: velocity, damage: damage, spawnTime: currentTime, lifetime: lifetime, effect: effect, isPersistent: isPersistent, pullStrength: pullStrength)
        p.setScale(1)
        p.xScale = projectileScale.dx
        p.yScale = projectileScale.dy
        p.color = color
        p.colorBlendFactor = 0.7
        return p
    }
}

final class BaseCannonWeapon: Weapon {
    init() {
        super.init(type: .baseCannon, fireRate: 0.1, damage: 50, color: .cyan, soundFile: "BaseCannon.wav")
    }

    override func fire(from position: CGPoint, texture: SKTexture, currentTime: TimeInterval) -> [Projectile] {
        lastFireTime = currentTime
        let p = makeProjectile(texture: texture, position: position, velocity: CGVector(dx: 0, dy: 600), currentTime: currentTime)
        return [p]
    }
}

final class SpreadCannonWeapon: Weapon {
    private static let sideOffset: CGFloat = 60

    init() {
        super.init(type: .spreadCannon, fireRate: 0.5, damage: 50, color: UIColor(red: 0.86, green: 0.44, blue: 0.58, alpha: 1), soundFile: "SpreadCannon.wav")
    }

    override func fire(from position: CGPoint, texture: SKTexture, currentTime: TimeInterval) -> [Projectile] {
        lastFireTime = currentTime
        let center = makeProjectile(texture: texture, position: position, velocity: CGVector(dx: 0, dy: 1200), currentTime: currentTime)
        let left = makeProjectile(texture: texture, position: CGPoint(x: position.x - Self.sideOffset, y: position.y), velocity: CGVector(dx: -90, dy: 1200), currentTime: currentTime)
        let right = makeProjectile(texture: texture, position: CGPoint(x: position.x + Self.sideOffset, y: position.y), velocity: CGVector(dx: 90, dy: 1200), currentTime: currentTime)
        return [center, left, right]
    }
}

final class IonCannonWeapon: Weapon {
    private static let sideOffset: CGFloat = 50

    init() {
        super.init(type: .ionCannon, fireRate: 1.0, damage: 100, color: UIColor(red: 0.68, green: 1.0, blue: 0.18, alpha: 1), soundFile: "IonCannon.wav")
    }

    override func fire(from position: CGPoint, texture: SKTexture, currentTime: TimeInterval) -> [Projectile] {
        lastFireTime = currentTime
        let velocity = CGVector(dx: 0, dy: 600)
        let left = makeProjectile(texture: texture, position: CGPoint(x: position.x - Self.sideOffset, y: position.y), velocity: velocity, currentTime: currentTime)
        let right = makeProjectile(texture: texture, position: CGPoint(x: position.x + Self.sideOffset, y: position.y), velocity: velocity, currentTime: currentTime)
        return [left, right]
    }
}

final class StasisRayWeapon: Weapon {
    init() {
        super.init(type: .stasisRay, fireRate: 0.5, damage: 0, color: UIColor(red: 0.27, green: 0.51, blue: 0.71, alpha: 1), soundFile: "IonCannon.wav", projectileScale: CGVector(dx: 0.1, dy: 0.35))
    }

    override func fire(from position: CGPoint, texture: SKTexture, currentTime: TimeInterval) -> [Projectile] {
        lastFireTime = currentTime
        let right = makeProjectile(texture: texture, position: position, velocity: CGVector(dx: 90, dy: 300), currentTime: currentTime, effect: .freeze)
        let left = makeProjectile(texture: texture, position: position, velocity: CGVector(dx: -90, dy: 300), currentTime: currentTime, effect: .freeze)
        return [left, right]
    }
}

final class GravityWellWeapon: Weapon {
    static let pullStrengthPerSecond: CGFloat = 30000

    init() {
        super.init(type: .gravityWell, fireRate: 3.0, damage: 500, color: UIColor(red: 0.28, green: 0.24, blue: 0.55, alpha: 1), soundFile: nil, projectileScale: CGVector(dx: 0.4, dy: 0.4))
    }

    override func fire(from position: CGPoint, texture: SKTexture, currentTime: TimeInterval) -> [Projectile] {
        lastFireTime = currentTime
        let p = makeProjectile(texture: texture, position: position, velocity: CGVector(dx: 0, dy: 150), currentTime: currentTime, lifetime: 10.0, isPersistent: true, pullStrength: Self.pullStrengthPerSecond)
        return [p]
    }
}

func makeWeapon(_ type: WeaponType) -> Weapon? {
    switch type {
    case .baseCannon: return BaseCannonWeapon()
    case .spreadCannon: return SpreadCannonWeapon()
    case .ionCannon: return IonCannonWeapon()
    case .stasisRay: return StasisRayWeapon()
    case .gravityWell: return GravityWellWeapon()
    case .shield: return nil // Shield is a defensive component, not a gun; see ShieldComponent.
    }
}
