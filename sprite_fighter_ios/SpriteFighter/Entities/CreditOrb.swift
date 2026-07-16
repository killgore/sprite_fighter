import SpriteKit

/// Dropped by every destroyed enemy. The player must fly through it to collect credits
/// before it despawns.
final class CreditOrb: SKSpriteNode {
    static let creditValue = 10
    static let lifetime: TimeInterval = 5.0
    static let fallSpeed: CGFloat = -210 // points/sec, downward

    let spawnTime: TimeInterval

    init(texture: SKTexture, position: CGPoint, currentTime: TimeInterval) {
        spawnTime = currentTime
        super.init(texture: texture, color: .green, size: texture.size())
        self.position = position
        colorBlendFactor = 0.7
        setScale(0.2)
    }

    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }

    func isExpired(at currentTime: TimeInterval) -> Bool {
        (currentTime - spawnTime) > CreditOrb.lifetime
    }

    func step(dt: TimeInterval) {
        position.y += CreditOrb.fallSpeed * CGFloat(dt)
    }
}
