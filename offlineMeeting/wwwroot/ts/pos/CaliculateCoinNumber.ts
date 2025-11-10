import { SetEventListner } from "@root/share/SetEventListner"

export class CaliculateCoinNumber {
    setCoin = () => {
        Array.from(document.getElementsByClassName("order")).forEach((element: Element) => {
            const htmlElement = <HTMLElement>element;

            const coinNumberElement = <HTMLInputElement>htmlElement.querySelector(".coin_number");
            const coinNumberShowElement = <HTMLInputElement>htmlElement.querySelector(".coin_number_show");
            // const coinNumberModalShowElement = <HTMLInputElement>htmlElement.querySelector(".coin_number_modal_show");

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
                    coinNumber = this.caliculate(htmlElement, (<HTMLElement>event.currentTarget).classList.contains('alcohol_order'));
                    coinNumberElement.value = coinNumber.toString();
                    coinNumberShowElement.innerText = coinNumber.toString();
                    // coinNumberModalShowElement.innerText = coinNumber.toString();
                }
            );

            SetEventListner.setEvent(
                <HTMLElement>element,
                "change",
                ".alcohol_amount",
                async (event: Event) => {
                    // (<HTMLInputElement>element.querySelector(".alcohol_amount_modal")).value = (<HTMLInputElement>event.target).value;
                    coinNumber = this.caliculate(htmlElement, true);
                    coinNumberElement.value = coinNumber.toString();
                    coinNumberShowElement.innerText = coinNumber.toString();
                    // coinNumberModalShowElement.innerText = coinNumber.toString();
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

            SetEventListner.setEvent(
                <HTMLElement>element,
                "change",
                ".alcohol_amount_modal",
                async (event: Event) => {
                    (<HTMLInputElement>element.querySelector(".alcohol_amount")).value = (<HTMLInputElement>event.target).value;
                    coinNumber = this.caliculateCoinNumber(htmlElement);
                    coinNumberElement.value = coinNumber.toString();
                    coinNumberShowElement.innerText = coinNumber.toString();
                    coinNumberModalShowElement.innerText = coinNumber.toString();
                }
            );
            */
        });
    }

    private caliculate = (htmlElement: HTMLElement, isAlcohol: boolean) =>  {
        if (isAlcohol) {
            return this.caliculateAlcoholCoinNumber(htmlElement);
        } else {
            return this.caliculateCoinNumber(htmlElement);
        }
    }

    private caliculateCoinNumber = (htmlElement: HTMLElement) => {
        const purhcase_coin_number = parseInt((<HTMLInputElement>htmlElement.querySelector(".purhcase_coin_number"))?.value ?? "0");
        const orderNumber = parseInt((<HTMLInputElement>htmlElement.querySelector(".order_number"))?.value ?? "0");

        if (orderNumber == 0) {
            return 0;
        }

        return purhcase_coin_number * orderNumber;
    }

    private caliculateAlcoholCoinNumber = (htmlElement: HTMLElement) => {
        const alcoholAmount = parseInt((<HTMLInputElement>htmlElement.querySelector(".alcohol_amount"))?.value ?? "0");
        const orderNumber = parseInt((<HTMLInputElement>htmlElement.querySelector(".order_number"))?.value ?? "0");
        const coinPer2shaku = parseInt((<HTMLInputElement>htmlElement.querySelector(".coin_per_2shaku"))?.value ?? "0");
        const leftAmount = parseInt((<HTMLInputElement>htmlElement.querySelector(".left_amount"))?.value ?? "0");

        let coinNumber = 0;
        let adjustmentCoin = 0;
        let orderAmount = 0;

        if (orderNumber == 0) {
            return 0;
        }

        if (alcoholAmount == 0) {
            orderAmount = Math.floor(leftAmount / 36);
            adjustmentCoin = coinPer2shaku * Math.floor(leftAmount / 180);
        } else {
            orderAmount = Math.floor(alcoholAmount / 36);
            adjustmentCoin = coinPer2shaku * Math.floor(alcoholAmount / 180);
        }

        coinNumber = (coinPer2shaku * orderAmount - adjustmentCoin) * orderNumber;

        return coinNumber;
    }
}