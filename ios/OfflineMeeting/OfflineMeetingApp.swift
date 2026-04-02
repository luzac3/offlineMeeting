import SwiftUI

@main
struct OfflineMeetingApp: App {
    @StateObject private var settings = AppSettings()
    @StateObject private var printService = PrintService()

    var body: some Scene {
        WindowGroup {
            ContentView()
                .environmentObject(settings)
                .environmentObject(printService)
        }
    }
}
