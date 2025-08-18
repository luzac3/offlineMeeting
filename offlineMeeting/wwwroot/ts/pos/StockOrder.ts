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
                    const resultId = parseInt(htmlElement.dataset.result_id!);
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

                    LocalStrage.set("OrderEntityList", orderEntityList);

                    window.alert("追加成功");
                }
            );
        });
    }
}