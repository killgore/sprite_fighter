import SpriteKit

final class PlayerShip: SKSpriteNode {
    static let maxLife = 1000

    var life = PlayerShip.maxLife
    var credits = 0
    var weapons: [Weapon] = []
    var shield: ShieldComponent?

    init(texture: SKTexture) {
        super.init(texture: texture, color: .clear, size: texture.size())
    }

    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }

    func takeDamage(_ dmg: Int) {
        life = max(0, life - dmg)
    }

    var isDead: Bool { life <= 0 }

    func hasWeapon(_ type: WeaponType) -> Bool {
        if type == .shield { return shield != nil }
        return weapons.contains { $0.type == type }
    }

    func reset() {
        life = PlayerShip.maxLife
        credits = 0
        weapons = [BaseCannonWeapon()]
        shield?.removeFromParent()
        shield = nil
    }
}
