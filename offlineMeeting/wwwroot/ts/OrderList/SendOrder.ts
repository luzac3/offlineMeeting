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
            const orderButton = <HTMLButtonElement>document.getElementById("send_order");
            // ボタンを殺す
            orderButton.disabled = true;

            const orderElements = Array.from(document.getElementsByClassName("order"));
            let orderEntityList: OrderEntity[] = [];

            if (orderElements.length > 0) {
                Array.from(document.getElementsByClassName("order")).forEach((orderElement) => {
                    const alcoholAmountElement = orderElement.querySelector(".alcohol_amount") as HTMLInputElement | null;
                    const alcoholAmount = alcoholAmountElement ? parseInt(alcoholAmountElement.value) : 0;

                    orderEntityList.push(new OrderEntity(
                        parseInt((<HTMLElement>orderElement).dataset.result_id!),
                        parseInt((<HTMLInputElement>orderElement.querySelector('.category'))?.value ?? "0"),
                        parseInt((<HTMLInputElement>orderElement.querySelector(".order_number")).value),
                        parseInt((<HTMLInputElement>orderElement.querySelector(".coin_number")).value),
                        alcoholAmount
                    ));
                });

                const orderObjectList = orderEntityList.map(order => ({
                    ResultId: order.ResultId,
                    Category: order.Category,
                    OrderNumber: order.OrderNumber,
                    OrderCoins: order.OrderCoins,
                    AlcoholAmount: order.AlcoholAmount
                })).filter(x => x.OrderNumber != 0);

                // 品切れチェック
                const resultIds = orderObjectList.map(x => x.ResultId);
                const soldOutIds = await this.checkSoldOut(resultIds);

                if (soldOutIds.length > 0) {
                    const soldOutNames = this.getSoldOutNames(soldOutIds);
                    const message = `以下の商品は品切れとなっております:\n${soldOutNames.join("\n")}\n\nよろしいですか？`;
                    if (!window.confirm(message)) {
                        orderButton.disabled = false;
                        return;
                    }
                }

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
                    orderButton.disabled = false;
                    window.alert(result.message);
                });
            }
        });
    }

    private checkSoldOut = async (resultIds: number[]): Promise<number[]> => {
        const fetchApi = new FetchApi();
        try {
            return await fetchApi.send(
                '/Pos/CheckSoldOut',
                'POST',
                this.headers,
                resultIds,
                'json'
            ) as number[];
        } catch {
            return [];
        }
    }

    private getSoldOutNames = (soldOutIds: number[]): string[] => {
        const names: string[] = [];
        soldOutIds.forEach(id => {
            const orderElement = document.querySelector(`.order[data-result_id="${id}"]`);
            if (orderElement) {
                const name =
                    orderElement.querySelector(".alcohol_name")?.textContent?.trim() ??
                    orderElement.querySelector(".food_name")?.textContent?.trim() ??
                    `ID: ${id}`;
                names.push(name);
            }
        });
        return names;
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