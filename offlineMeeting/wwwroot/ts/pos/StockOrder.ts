import { SetEventListner } from "@root/share/SetEventListner"
import { LocalStrage } from "@root/share/LocalStrage"
import { OrderEntity } from "./OrderEntity"

export class StockOrder {
    addOrder = () => {
        Array.from(document.getElementsByClassName("alcohol_order")).forEach((element: Element) => {
            const htmlElement = <HTMLElement>element;

            SetEventListner.setEvent(
                <HTMLElement>element,
                "click",
                ".order_button",
                async (event: Event) => {
                    const resultId = parseInt((<HTMLElement>htmlElement.querySelector('.alchol_info')).dataset.result_id!);
                    const alcoholAmount = parseInt((<HTMLInputElement>htmlElement.querySelector(".alcohol_amount"))?.value ?? "0");
                    const orderNumber = parseInt((<HTMLInputElement>htmlElement.querySelector(".order_number"))?.value ?? "0");
                    const coinNumber = parseInt((<HTMLInputElement>htmlElement.querySelector(".coin_number"))?.value ?? "0");

                    let orderEntityList = [];

                    const orderEntity = new OrderEntity(
                        resultId,
                        orderNumber,
                        coinNumber,
                        alcoholAmount
                    );

                    if (LocalStrage.check("OrderEntityList")) {
                        orderEntityList = LocalStrage.get("OrderEntityList");
                    }

                    orderEntityList.push(orderEntity);

                    // 二回以上注文されていた場合、数量を合計して一つにまとめる
                    LocalStrage.set("OrderEntityList", this.Deduplication(orderEntityList));

                    window.alert("追加成功");
                }
            );
        });
    }

    private Deduplication = (array: OrderEntity[]) => {
        // IDごとに合計
        const map = new Map<string, OrderEntity>();

        array.forEach(item => {
            const key = `${item.ResultId}_${item.AlcoholAmount}`;
            if (map.has(key)) {
                map.get(key)!.OrderNumber += item.OrderNumber;
            } else {
                map.set(key, { ...item });
            }
        });

        return Array.from(map.values());
    }
}