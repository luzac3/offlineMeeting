import { LocalStrage } from "@root/share/LocalStrage"
import { FetchApi } from "@root/share/FetchApi"

export class SendOrder {
    private url: string;
    private method: string;
    private headers: { [key: string]: string }; 
    private responseKind: string;

    constructor() {
        this.url = '/Pos/SendOrder';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }

    setOrderList = () => {
        document.getElementById("send_order")?.addEventListener("click", async () => {
            let orderEntityList: { [key: string]: number }[] = [];

            if (LocalStrage.check("OrderEntityList")) {
                orderEntityList = LocalStrage.get("OrderEntityList");
            }

            this.send(orderEntityList).then((data: string) => {
                window.alert("注文を送信しました");
            });
        });
    }

    private send = async (orderEntityList: { [key: string]: number }[]) => {
        const fetchApi = new FetchApi();

        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            orderEntityList,
            this.responseKind
        ).then(async (data: string) => {
            return data;
        }).catch(e => {
            throw e;
        });
    }
}