import SwiftUI

/// Electron の menu.html に相当するメニュー画面
struct ContentView: View {
    @EnvironmentObject private var settings: AppSettings
    @EnvironmentObject private var printService: PrintService
    @State private var openedURL: IdentifiableURL?
    @State private var showSettings = false

    var body: some View {
        NavigationStack {
            VStack(spacing: 24) {
                Text("百酒会")
                    .font(.largeTitle)
                    .fontWeight(.bold)
                    .padding(.top, 48)

                ForEach(settings.menuItems, id: \.url) { item in
                    Button {
                        openedURL = IdentifiableURL(url: URL(string: item.url)!)
                    } label: {
                        Text(item.name)
                            .font(.title2)
                            .frame(maxWidth: .infinity)
                            .padding()
                            .background(Color.white)
                            .overlay(
                                RoundedRectangle(cornerRadius: 8)
                                    .stroke(Color.primary, lineWidth: 2)
                            )
                    }
                    .padding(.horizontal, 32)
                }

                Spacer()

                Button {
                    showSettings = true
                } label: {
                    HStack {
                        Image(systemName: "gearshape")
                        Text("設定")
                    }
                    .font(.body)
                    .padding(.horizontal, 24)
                    .padding(.vertical, 10)
                    .background(Color(UIColor.secondarySystemBackground))
                    .cornerRadius(8)
                }
                .padding(.bottom, 24)
            }
            .background(Color(UIColor.systemGroupedBackground))
            .fullScreenCover(item: $openedURL) { item in
                WebPageView(url: item.url)
                    .environmentObject(settings)
                    .environmentObject(printService)
                    .ignoresSafeArea()
            }
            .sheet(isPresented: $showSettings) {
                SettingsView()
                    .environmentObject(settings)
                    .environmentObject(printService)
            }
        }
    }
}

/// Identifiable wrapper for URL
struct IdentifiableURL: Identifiable {
    let id = UUID()
    let url: URL
}
