export class SetEventListener {
    static setEvent = (
        parentElement: HTMLElement | undefined | null,
        type: keyof HTMLElementEventMap,
        selector: string, // targetName というより selector なので名前変更
        // 第2引数で「実際にマッチした要素」を返してあげると jQueryの this のように使えて超便利
        callback: (event: Event, targetElement: HTMLElement) => void,
        atOnce: boolean = false
    ) => {
        if (!parentElement) return null;

        const handler = (event: Event) => {
            const clickedElement = event.target as Element;

            // クリックされた要素自身、またはその親を遡ってセレクタに一致する要素を探す
            const matchedElement = clickedElement.closest(selector) as HTMLElement;

            // マッチした要素が存在し、かつ parentElement の管轄内である場合のみ発火
            if (matchedElement && parentElement.contains(matchedElement)) {

                // コールバックに「発火したEvent」と「マッチした要素」の両方を渡す
                callback(event, matchedElement);

                if (atOnce) {
                    parentElement.removeEventListener(type, handler);
                }
            }
        }

        parentElement.addEventListener(type, handler);
        return handler;
    }

    // fixme　呼び出し元からSetEventAllを消していくこと
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
