import SpriteKit

final class StarField {
    private var stars: [(node: SKSpriteNode, speed: CGFloat)] = []
    private let bounds: CGRect

    init(parent: SKNode, texture: SKTexture, bounds: CGRect) {
        self.bounds = bounds
        addLayer(count: 15, parent: parent, texture: texture, scale: 1.0, speed: 240, zPosition: -3)
        addLayer(count: 20, parent: parent, texture: texture, scale: 0.6, speed: 120, zPosition: -4)
        addLayer(count: 35, parent: parent, texture: texture, scale: 0.35, speed: 60, zPosition: -5)
    }

    private func addLayer(count: Int, parent: SKNode, texture: SKTexture, scale: CGFloat, speed: CGFloat, zPosition: CGFloat) {
        for _ in 0..<count {
            let star = SKSpriteNode(texture: texture)
            star.setScale(scale)
            star.zPosition = zPosition
            star.position = CGPoint(x: CGFloat.random(in: bounds.minX...bounds.maxX),
                                     y: CGFloat.random(in: bounds.minY...bounds.maxY))
            parent.addChild(star)
            stars.append((star, speed))
        }
    }

    func update(dt: TimeInterval) {
        guard dt > 0 else { return }
        for (star, speed) in stars {
            star.position.y -= speed * CGFloat(dt)
            if star.position.y < bounds.minY {
                star.position.y = bounds.maxY
                star.position.x = CGFloat.random(in: bounds.minX...bounds.maxX)
            }
        }
    }
}
