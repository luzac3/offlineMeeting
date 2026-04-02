# OfflineMeeting iOS

Electron版と同等の機能を持つiOS (iPad) ネイティブアプリ。

## 機能
- **メニュー画面**: setting.json に基づくページ遷移ボタン
- **WebView**: WKWebView でWebアプリを表示
- **JSブリッジ**: `window.webkit.messageHandlers` 経由で印刷指示を受信
- **サイレント印刷**: 初回のみプリンタ選択、以降は保存したプリンタへ自動印刷
- **設定画面**: URL・プリンタをアプリ内から変更可能

## 前提条件
- macOS + Xcode 15以上
- Homebrew (`brew`)

## セットアップ & ビルド

```bash
# 初回セットアップ (XcodeGen インストール + プロジェクト生成)
cd ios
chmod +x setup.sh build.sh
./setup.sh

# シミュレータでビルド
./build.sh simulator

# シミュレータでビルド&起動
./build.sh run

# 実機用ビルド
./build.sh device

# リリース用アーカイブ
./build.sh archive

# クリーン
./build.sh clean
```

## Xcode で直接開く場合
```bash
cd ios
./setup.sh          # project.yml から .xcodeproj を生成
open OfflineMeeting.xcodeproj
```

## 設定ファイル (setting.json)
Electron版と同じフォーマット。初回起動時にアプリ内 UserDefaults へコピーされ、
以降は設定画面から編集可能。
```json
{
  "orderUrl": "https://sake.wolfslab.jp/Pos/UserOrders?eid=60473",
  "orderName": "チュウモンイチラン",
  "printUrlBase": "https://sake.wolfslab.jp/Pos/PrintOrders?eid=60473&resultId=",
  ...
}
```
