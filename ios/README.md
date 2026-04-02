# OfflineMeeting iOS

Electron版と同等の機能を持つiOS (iPad) ネイティブアプリ。

## 機能
- **メニュー画面**: setting.json に基づくページ遷移ボタン
- **WebView**: WKWebView でWebアプリを表示
- **JSブリッジ**: `window.electronAPI` 互換インターフェース（Web側の変更不要）
- **サイレント印刷**: 初回のみプリンタ選択、以降は保存したプリンタへ自動印刷

## セットアップ
1. Xcode で新規プロジェクト作成: **App** → Interface: **SwiftUI** → Language: **Swift**
2. Product Name: `OfflineMeeting`
3. `OfflineMeeting/` 配下のSwiftファイルを全てプロジェクトに追加
4. `Resources/setting.json` をプロジェクトに追加（Copy items if needed にチェック）
5. `Info.plist` に以下を追加:
   - `NSLocalNetworkUsageDescription`: プリンタ検索のためにローカルネットワークへのアクセスが必要です
   - `NSBonjourServices`: `_ipp._tcp`, `_ipps._tcp` (AirPrint用)
   - `App Transport Security Settings` → `Allow Arbitrary Loads`: YES (開発用、本番ではドメイン指定)

## 設定ファイル (setting.json)
Electron版と同じフォーマット:
```json
{
  "orderUrl": "https://sake.wolfslab.jp/Pos/UserOrders?eid=60473",
  "orderName": "チュウモンイチラン",
  "printUrlBase": "https://sake.wolfslab.jp/Pos/PrintOrders?eid=60473&resultId=",
  ...
}
```
