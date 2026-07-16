import SpriteKit

final class ShieldComponent: SKSpriteNode {
    static let maxHealth = 500
    static let damagePerHit = 50

    var health = ShieldComponent.maxHealth

    init(texture: SKTexture) {
        super.init(texture: texture, color: .yellow, size: texture.size())
        colorBlendFactor = 0.4
        setScale(2.0)
    }

    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }

    func absorbHit() {
        health -= ShieldComponent.damagePerHit
    }

    var isDestroyed: Bool { health <= 0 }
}
