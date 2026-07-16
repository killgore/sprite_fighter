import Foundation

enum EnemyKind {
    case grunt
    case sidewinder
    case striker
    case juggernaut
}

/// Checked high-threshold-first: the first rule where a 0..<100 roll exceeds `threshold` wins,
/// mirroring the original game's stacked if/else-if spawn chances.
struct SpawnRule {
    let threshold: Int
    let kind: EnemyKind
}

struct LevelConfig {
    let killTarget: Int
    let spawnTable: [SpawnRule]

    func rollSpawn() -> EnemyKind? {
        let roll = Int.random(in: 0..<100)
        for rule in spawnTable where roll > rule.threshold {
            return rule.kind
        }
        return nil
    }
}

enum Levels {
    static let all: [LevelConfig] = [
        LevelConfig(killTarget: 25, spawnTable: [
            SpawnRule(threshold: 96, kind: .grunt)
        ]),
        LevelConfig(killTarget: 50, spawnTable: [
            SpawnRule(threshold: 96, kind: .grunt),
            SpawnRule(threshold: 92, kind: .sidewinder)
        ]),
        LevelConfig(killTarget: 75, spawnTable: [
            SpawnRule(threshold: 98, kind: .grunt),
            SpawnRule(threshold: 95, kind: .sidewinder),
            SpawnRule(threshold: 92, kind: .striker),
            SpawnRule(threshold: 90, kind: .juggernaut)
        ])
    ]

    static let maxLevel = all.count

    static func config(for level: Int) -> LevelConfig {
        all[min(max(level, 1), maxLevel) - 1]
    }
}
