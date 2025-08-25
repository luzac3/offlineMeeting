import { LocalStrage } from "@root/share/LocalStrage"
import { FetchApi } from "@root/share/FetchApi"
import { OrderEntity } from "../pos/OrderEntity"

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
            const orderElements = Array.from(document.getElementsByClassName("order"));
            let orderEntityList: OrderEntity[] = [];

            if (orderElements.length > 0) {
                Array.from(document.getElementsByClassName("order")).forEach((orderElement) => {
                    orderEntityList.push(new OrderEntity(
                        parseInt((<HTMLElement>orderElement).dataset.result_id!),
                        parseInt((<HTMLInputElement>orderElement.querySelector(".order_number")).value),
                        parseInt((<HTMLInputElement>orderElement.querySelector(".order_coins")).value),
                        parseInt((<HTMLInputElement>orderElement.querySelector(".alcohol_amount")).value)
                    ));
                });

                const orderObjectList = orderEntityList.map(order => ({
                    ResultId: order.ResultId,
                    OrderNumber: order.OrderNumber,
                    OrderCoins: order.OrderCoins,
                    AlcoholAmount: order.AlcoholAmount
                })).filter(x => x.OrderNumber != 0);

                this.send(orderObjectList).then((data: string) => {
                    const result = JSON.parse(data) as { [key: string]: string };
                    if (result.status == "200") {
                        // オーダーのストックをクリア
                        LocalStrage.delete();
                        // 画面をクリア
                        document.getElementById("order_list")!.innerHTML = "";
                        document.getElementById("pon_coins")!.innerText = "0";
                        (<HTMLInputElement>document.getElementById("pon_coins_input")!).value = "0";
                    }
                    window.alert(result.message);
                });
            }
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