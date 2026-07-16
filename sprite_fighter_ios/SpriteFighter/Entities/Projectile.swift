import SpriteKit

enum ProjectileEffect {
    case none
    case freeze
}

final class Projectile: SKSpriteNode {
    var velocity: CGVector
    let damage: Int
    let spawnTime: TimeInterval
    let lifetime: TimeInterval
    let effect: ProjectileEffect
    /// Gravity well bolts pass through hits instead of being destroyed, and pull enemies toward themselves.
    let isPersistent: Bool
    let pullStrength: CGFloat

    init(texture: SKTexture, position: CGPoint, velocity: CGVector, damage: Int, spawnTime: TimeInterval, lifetime: TimeInterval, effect: ProjectileEffect = .none, isPersistent: Bool = false, pullStrength: CGFloat = 0) {
        self.velocity = velocity
        self.damage = damage
        self.spawnTime = spawnTime
        self.lifetime = lifetime
        self.effect = effect
        self.isPersistent = isPersistent
        self.pullStrength = pullStrength
        super.init(texture: texture, color: .clear, size: texture.size())
        self.position = position
    }

    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }

    func isExpired(at time: TimeInterval) -> Bool {
        (time - spawnTime) > lifetime
    }

    func step(dt: TimeInterval) {
        position.x += velocity.dx * CGFloat(dt)
        position.y += velocity.dy * CGFloat(dt)
    }
}
