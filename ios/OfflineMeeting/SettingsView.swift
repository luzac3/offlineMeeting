import SwiftUI

/// Electron の setting.json をGUIで編集する設定画面
struct SettingsView: View {
    @EnvironmentObject private var settings: AppSettings
    @EnvironmentObject private var printService: PrintService
    @Environment(\.dismiss) private var dismiss

    var body: some View {
        NavigationStack {
            Form {
                // --- 注文画面 ---
                Section("注文画面") {
                    LabeledField("表示名", text: $settings.orderName)
                    LabeledField("URL", text: $settings.orderUrl)
                        .keyboardType(.URL)
                        .textInputAutocapitalization(.never)
                }

                // --- チェックイン画面 ---
                Section("チェックイン画面") {
                    LabeledField("表示名", text: $settings.checkinName)
                    LabeledField("URL", text: $settings.checkinUrl)
                        .keyboardType(.URL)
                        .textInputAutocapitalization(.never)
                }

                // --- 印刷 ---
                Section("印刷") {
                    LabeledField("注文印刷 URL ベース", text: $settings.printUrlBase)
                        .keyboardType(.URL)
                        .textInputAutocapitalization(.never)
                    LabeledField("QR印刷 URL ベース", text: $settings.checkinPrintUrlBase)
                        .keyboardType(.URL)
                        .textInputAutocapitalization(.never)
                }

                // --- プリンタ ---
                Section("プリンタ") {
                    HStack {
                        Text("選択中")
                        Spacer()
                        Text(printService.savedPrinterName)
                            .foregroundStyle(.secondary)
                    }

                    Button("プリンタを選択") {
                        printService.showPrinterPicker()
                    }

                    if printService.savedPrinter != nil {
                        Button("プリンタ設定をクリア", role: .destructive) {
                            printService.clearPrinter()
                        }
                    }
                }
            }
            .navigationTitle("設定")
            .navigationBarTitleDisplayMode(.inline)
            .toolbar {
                ToolbarItem(placement: .confirmationAction) {
                    Button("保存") {
                        settings.save()
                        dismiss()
                    }
                }
                ToolbarItem(placement: .cancellationAction) {
                    Button("キャンセル") {
                        dismiss()
                    }
                }
            }
        }
    }
}

/// テキストフィールド付きラベル
private struct LabeledField: View {
    let label: String
    @Binding var text: String

    init(_ label: String, text: Binding<String>) {
        self.label = label
        self._text = text
    }

    var body: some View {
        VStack(alignment: .leading, spacing: 4) {
            Text(label)
                .font(.caption)
                .foregroundStyle(.secondary)
            TextField(label, text: $text)
                .textFieldStyle(.roundedBorder)
        }
    }
}
