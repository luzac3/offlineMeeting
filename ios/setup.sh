#!/bin/bash
set -e
cd "$(dirname "$0")"

echo "=== OfflineMeeting iOS セットアップ ==="

# XcodeGen インストール確認
if ! command -v xcodegen &> /dev/null; then
    echo "XcodeGen をインストール中..."
    brew install xcodegen
else
    echo "XcodeGen: $(xcodegen --version)"
fi

# Xcode CLI tools 確認
if ! xcode-select -p &> /dev/null; then
    echo "Xcode Command Line Tools をインストール中..."
    xcode-select --install
    echo "インストール完了後、再度このスクリプトを実行してください"
    exit 1
fi

# Xcode プロジェクト生成
echo "Xcode プロジェクトを生成中..."
xcodegen generate

echo ""
echo "=== セットアップ完了 ==="
echo "  Xcode で開く:  open OfflineMeeting.xcodeproj"
echo "  ビルド:        ./build.sh"
echo "  シミュレータ:   ./build.sh simulator"
echo "  実機:          ./build.sh device"
