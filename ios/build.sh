#!/bin/bash
set -e
cd "$(dirname "$0")"

SCHEME="OfflineMeeting"
PROJECT="OfflineMeeting.xcodeproj"
MODE="${1:-simulator}"

# プロジェクトが無ければ生成
if [ ! -d "$PROJECT" ]; then
    echo "Xcode プロジェクトが見つかりません。生成します..."
    ./setup.sh
fi

case "$MODE" in
    simulator)
        echo "=== シミュレータビルド (iPad) ==="
        DESTINATION="platform=iOS Simulator,name=iPad (10th generation)"
        xcodebuild \
            -project "$PROJECT" \
            -scheme "$SCHEME" \
            -destination "$DESTINATION" \
            -configuration Debug \
            build \
            | tail -20
        echo "=== ビルド完了 ==="
        ;;

    device)
        echo "=== 実機ビルド ==="
        xcodebuild \
            -project "$PROJECT" \
            -scheme "$SCHEME" \
            -destination "generic/platform=iOS" \
            -configuration Debug \
            build \
            | tail -20
        echo "=== ビルド完了 ==="
        echo "※ 実機インストールは Xcode から行ってください"
        ;;

    archive)
        echo "=== アーカイブ (リリース用) ==="
        ARCHIVE_PATH="build/${SCHEME}.xcarchive"
        xcodebuild \
            -project "$PROJECT" \
            -scheme "$SCHEME" \
            -destination "generic/platform=iOS" \
            -configuration Release \
            -archivePath "$ARCHIVE_PATH" \
            archive \
            | tail -20
        echo "=== アーカイブ完了: $ARCHIVE_PATH ==="
        echo "TestFlight配布: Xcode > Window > Organizer から"
        ;;

    run)
        echo "=== シミュレータで実行 ==="
        DESTINATION="platform=iOS Simulator,name=iPad (10th generation)"
        # ビルド & シミュレータ起動
        xcodebuild \
            -project "$PROJECT" \
            -scheme "$SCHEME" \
            -destination "$DESTINATION" \
            -configuration Debug \
            build \
            | tail -5

        # シミュレータ起動
        BUNDLE_ID="jp.wolfslab.offlinemeeting"
        xcrun simctl boot "iPad (10th generation)" 2>/dev/null || true
        open -a Simulator
        sleep 2

        # アプリインストール & 起動
        APP_PATH=$(find ~/Library/Developer/Xcode/DerivedData -name "${SCHEME}.app" -path "*/Debug-iphonesimulator/*" | head -1)
        if [ -n "$APP_PATH" ]; then
            xcrun simctl install booted "$APP_PATH"
            xcrun simctl launch booted "$BUNDLE_ID"
            echo "=== シミュレータで起動しました ==="
        else
            echo "エラー: ビルド成果物が見つかりません"
            exit 1
        fi
        ;;

    clean)
        echo "=== クリーン ==="
        xcodebuild \
            -project "$PROJECT" \
            -scheme "$SCHEME" \
            clean
        rm -rf build/
        echo "=== クリーン完了 ==="
        ;;

    *)
        echo "使い方: ./build.sh [simulator|device|archive|run|clean]"
        echo ""
        echo "  simulator  シミュレータ用ビルド (デフォルト)"
        echo "  device     実機用ビルド"
        echo "  archive    リリース用アーカイブ (.xcarchive)"
        echo "  run        シミュレータでビルド&起動"
        echo "  clean      ビルド成果物を削除"
        exit 1
        ;;
esac
