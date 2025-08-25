export class FetchApi {
    send = async (
        url: string,
        method: string,
        headers: { [key: string]: string },
        body: {
            [key: string]:
            string |
            string[] |
            string[][] |
            { [key: string]: string } |
            { [ket: string]: boolean } |
            { [key: string]: string }[] |
            { [key: string]: boolean }[] |
            Uint8Array |
            number |
            number[] |
            boolean |
            FormDataEntryValue |
            null
        } | { [key: string]: number }[] | string | null,
        //body: any,
        responseKind: string = "json",
        needResponseData: boolean = true
    ) => {
        let request;

        if (method == "GET") {
            request = new Request(url, {
                method: method,
                headers: headers
            });
        } else {
            request = new Request(url, {
                method: method,
                headers: headers,
                body: JSON.stringify(body)
            });
        }

        const response = await fetch(request).catch(e => { throw e });

        if (!response.ok) {
            throw new Error(response.statusText);
        }

        if (!needResponseData) {
            return;
        }

        switch (responseKind) {
            case "json":
                return await this.fetchResonse(response.json()).catch(e => { throw e });
            case "text":
                return await this.fetchResonse(response.text()).catch(e => { throw e });
        }
    }

    private async fetchResonse(response: Promise<any>) {
        return await response.then(data => {
            return data;
        })
        .catch(error => {
            // エラー処理
            console.log(error);
            throw error;
        });
    }
}
