import { ControlNavbar } from "@root/share/ControlNavbar";
import { Counter } from "@root/share/Counter";
import { LocalStrage } from "@root/share/LocalStrage"
import { FetchApi } from "@root/share/FetchApi"

export class GetOrderList {
    private url: string;
    private method: string;
    private headers: { [key: string]: string }; 
    private responseKind: string;

    constructor() {
        this.url = '/Pos/OrderListContents';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }

    setOrderList = () => {
        const counter = new Counter();
        let orderEntityList: { [key: string]: number }[] = [];

        if (LocalStrage.check("OrderEntityList")) {
            orderEntityList = LocalStrage.get("OrderEntityList");
        }

        let totalCoins = 0;

        this.send(orderEntityList).then((data: string) => {
            document.getElementById("order_list")!.innerHTML = data;
            document.getElementById("pon_coins")!.innerText = totalCoins.toString();
            (<HTMLInputElement>document.getElementById("pon_coins_input")!).value = totalCoins.toString();
            // カウンターをアクティブ
            counter.SetCounter();
            // ナビバーの位置を調整
            new ControlNavbar();
        });

        orderEntityList.forEach(order => {
            totalCoins += order.OrderCoins;
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