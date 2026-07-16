import SwiftUI
import SpriteKit

@main
struct SpriteFighterApp: App {
    var body: some Scene {
        WindowGroup {
            GeometryReader { proxy in
                GameView(size: proxy.size)
            }
            .ignoresSafeArea()
            .statusBarHidden(true)
        }
    }
}

struct GameView: UIViewRepresentable {
    let size: CGSize

    func makeUIView(context: Context) -> SKView {
        let view = SKView(frame: CGRect(origin: .zero, size: size))
        view.ignoresSiblingOrder = true
        let scene = GameScene(size: size)
        scene.scaleMode = .resizeFill
        view.presentScene(scene)
        return view
    }

    func updateUIView(_ uiView: SKView, context: Context) {}
}
