export class FetchApi {
    send = async <TBody = unknown, TResponse = any>(
        url: string,
        method: string,
        headers: { [key: string]: string },
        body: TBody,
        responseKind: string = "json",
        needResponseData: boolean = true
    ): Promise<TResponse | void> => {

        const request = new Request(url, {
            method: method,
            headers: headers,
            // GET以外で、bodyが存在する場合のみJSON化する
            body: method !== "GET" && body !== undefined ? JSON.stringify(body) : null
        });

        ManageLoadElement.set();

        try {
            // fetch自体のネットワークエラー等もここで受ける
            const response = await fetch(request);

            if (!response.ok) {
                // HTTPエラー（400, 500など）
                throw new Error(response.statusText);
            }

            if (!needResponseData) {
                return;
            }

            // response.json() 自体がPromiseを返すので、そのままawaitして返すだけでOK
            switch (responseKind) {
                case "json":
                    return await response.json() as TResponse;
                case "text":
                    return await response.text() as unknown as TResponse;
            }
        } catch (error) {
            console.error("FetchError:", error);
            throw error; // そのまま上に投げる
        } finally {
            // 必ず最後に1回だけここを通ってローディングを解除
            ManageLoadElement.remove();
        }
    }
}

class  ManageLoadElement {
    static loadElement = document.getElementById("loading-overlay") as HTMLElement;
    static set = () => {
        if (this.loadElement == null) {
            return;
        }
        this.loadElement.classList.remove("nodisplay");
    }

    static remove = () => {
        if (this.loadElement == null) {
            return;
        }
        this.loadElement.classList.add("nodisplay");
    }
}