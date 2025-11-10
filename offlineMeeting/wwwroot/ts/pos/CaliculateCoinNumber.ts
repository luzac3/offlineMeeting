import { SetEventListner } from "@root/share/SetEventListner"

export class CaliculateCoinNumber {
    setCoin = () => {
        Array.from(document.getElementsByClassName("purchase_order")).forEach((element: Element) => {
            const htmlElement = <HTMLElement>element;

            const coinNumberElement = <HTMLInputElement>htmlElement.querySelector(".coin_number");
            const coinNumberShowElement = <HTMLInputElement>htmlElement.querySelector(".coin_number_show");
            const coinNumberModalShowElement = <HTMLInputElement>htmlElement.querySelector(".coin_number_modal_show");

            let coinNumber = 0;

            if (coinNumberElement == null) {
                return;
            }

            SetEventListner.setEvent(
                <HTMLElement>element,
                "change",
                ".order_number",
                async (event: Event) => {
                    //(<HTMLInputElement>element.querySelector(".order_number_modal")).value = (<HTMLInputElement>event.target).value;
                    coinNumber = this.caliculateCoinNumber(htmlElement);
                    coinNumberElement.value = coinNumber.toString();
                    coinNumberShowElement.innerText = coinNumber.toString();
                    coinNumberModalShowElement.innerText = coinNumber.toString();
                }
            );
            /*
            SetEventListner.setEvent(
                <HTMLElement>element,
                "change",
                ".order_number_modal",
                async (event: Event) => {
                    (<HTMLInputElement>element.querySelector(".order_number")).value = (<HTMLInputElement>event.target).value;
                    coinNumber = this.caliculateCoinNumber(htmlElement);
                    coinNumberElement.value = coinNumber.toString();
                    coinNumberShowElement.innerText = coinNumber.toString();
                    coinNumberModalShowElement.innerText = coinNumber.toString();
                }
            );
            */
        });
    }

    caliculateCoinNumber = (htmlElement: HTMLElement) => {
        const coinNumber = parseInt((<HTMLInputElement>htmlElement.querySelector(".coin_number"))?.value ?? "0");
        const orderNumber = parseInt((<HTMLInputElement>htmlElement.querySelector(".order_number"))?.value ?? "0");

        if (orderNumber == 0) {
            return 0;
        }

        return coinNumber * orderNumber;
    }
}