export class SetEventListener {
    static setEvent = (
        parentElement: HTMLElement | undefined | null,
        type: keyof HTMLElementEventMap,
        selector: string, // targetName というより selector なので名前変更
        // 【追加】第2引数で「実際にマッチした要素」を返してあげると jQueryの this のように使えて超便利
        callback: (event: Event, targetElement: HTMLElement) => void,
        atOnce: boolean = false
    ) => {
        if (!parentElement) return null;

        const handler = (event: Event) => {
            const clickedElement = event.target as Element;

            // 【改善①】closest() を使う
            // クリックされた要素自身、またはその親を遡ってセレクタに一致する要素を探す
            const matchedElement = clickedElement.closest(selector) as HTMLElement;

            // 【改善②】マッチした要素が存在し、かつ parentElement の管轄内である場合のみ発火
            if (matchedElement && parentElement.contains(matchedElement)) {

                // コールバックに「発火したEvent」と「マッチした要素」の両方を渡す
                callback(event, matchedElement);

                // 【称賛】ここの手動 remove は大正解！
                if (atOnce) {
                    parentElement.removeEventListener(type, handler);
                }
            }
        }

        parentElement.addEventListener(type, handler);
        return handler;
    }

    // 【改善③】イベント委譲において、実は単数と複数を分ける必要はありません！
    // 親要素で受け止める仕組み上、子要素が1個でも100個でも上記の setEvent だけで完結します。
    // 過去のコードとの互換性のために残し、内部で setEvent を呼ぶだけにします。
    static setEventAll = (
        parentElement: HTMLElement | undefined | null,
        type: keyof HTMLElementEventMap,
        selector: string,
        callback: (event: Event, targetElement: HTMLElement) => void,
        atOnce: boolean = false
    ) => {
        return this.setEvent(parentElement, type, selector, callback, atOnce);
    }

    static removeEvent = (
        parentElement: HTMLElement | undefined | null,
        type: keyof HTMLElementEventMap,
        handler: EventListenerOrEventListenerObject | null // 戻り値の型に合わせる
    ) => {
        if (!parentElement || !handler) return;
        parentElement.removeEventListener(type, handler);
    }
}

/*
// 呼び出し側のイメージ
SetEventListener.setEvent(document.body, "click", ".delete-btn", (e, target) => {
    // 昔の jQuery でいうところの $(this) が target としてそのまま使える
    const id = target.getAttribute("data-id");
    console.log(`${id} を削除します`);
});
*/