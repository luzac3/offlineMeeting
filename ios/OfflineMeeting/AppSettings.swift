import Foundation

/// Electron の setting.json と同じ構造
/// Bundle の setting.json を初期値として読み込み、ユーザー編集分は UserDefaults に保存
class AppSettings: ObservableObject {
    @Published var orderUrl: String
    @Published var orderName: String
    @Published var checkinUrl: String
    @Published var checkinName: String
    @Published var printUrlBase: String
    @Published var checkinPrintUrlBase: String

    private static let defaults = UserDefaults.standard
    private static let isInitializedKey = "AppSettings_initialized"

    init() {
        // 初回起動時のみ Bundle の setting.json から UserDefaults へコピー
        if !Self.defaults.bool(forKey: Self.isInitializedKey) {
            Self.loadFromBundle()
        }

        orderUrl = Self.defaults.string(forKey: "orderUrl") ?? ""
        orderName = Self.defaults.string(forKey: "orderName") ?? ""
        checkinUrl = Self.defaults.string(forKey: "checkinUrl") ?? ""
        checkinName = Self.defaults.string(forKey: "checkinName") ?? ""
        printUrlBase = Self.defaults.string(forKey: "printUrlBase") ?? ""
        checkinPrintUrlBase = Self.defaults.string(forKey: "checkinPrintUrlBase") ?? ""
    }

    /// メニュー項目を生成（Electron の SetElectron.ts と同じロジック）
    var menuItems: [(name: String, url: String)] {
        var items: [(String, String)] = []
        if !orderUrl.isEmpty, !orderName.isEmpty {
            items.append((orderName, orderUrl))
        }
        if !checkinUrl.isEmpty, !checkinName.isEmpty {
            items.append((checkinName, checkinUrl))
        }
        return items
    }

    func save() {
        Self.defaults.set(orderUrl, forKey: "orderUrl")
        Self.defaults.set(orderName, forKey: "orderName")
        Self.defaults.set(checkinUrl, forKey: "checkinUrl")
        Self.defaults.set(checkinName, forKey: "checkinName")
        Self.defaults.set(printUrlBase, forKey: "printUrlBase")
        Self.defaults.set(checkinPrintUrlBase, forKey: "checkinPrintUrlBase")
    }

    /// Bundle の setting.json → UserDefaults（初回のみ）
    private static func loadFromBundle() {
        guard let url = Bundle.main.url(forResource: "setting", withExtension: "json"),
              let data = try? Data(contentsOf: url),
              let json = try? JSONDecoder().decode(BundleSettings.self, from: data) else { return }

        defaults.set(json.orderUrl ?? "", forKey: "orderUrl")
        defaults.set(json.orderName ?? "", forKey: "orderName")
        defaults.set(json.checkinUrl ?? "", forKey: "checkinUrl")
        defaults.set(json.checkinName ?? "", forKey: "checkinName")
        defaults.set(json.printUrlBase ?? "", forKey: "printUrlBase")
        defaults.set(json.checkinPrintUrlBase ?? "", forKey: "checkinPrintUrlBase")
        defaults.set(true, forKey: isInitializedKey)
    }

    /// Bundle JSON デコード用（読み取り専用）
    private struct BundleSettings: Codable {
        let orderUrl: String?
        let orderName: String?
        let checkinUrl: String?
        let checkinName: String?
        let printUrlBase: String?
        let checkinPrintUrlBase: String?
    }
}
