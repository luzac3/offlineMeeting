export class CaliculateTotalCoins {
    constructor() {
        this.Set();
    }

    SetTotalCoins = () => {
        const orderListElement = document.getElementById("order_list") as HTMLElement;
        orderListElement.addEventListener("change", () => {
            this.Set();
        });
    }

    private Set = () => {
        const coins = Array.from(document.getElementsByClassName("coin_number"))
            .map(coinNumberElement => parseInt((<HTMLInputElement>coinNumberElement).value))
            .reduce((sum, element) => sum + element, 0);
        (<HTMLInputElement>document.getElementById("pon_coins_input")).value! = coins.toString();
        (<HTMLElement>document.getElementById("pon_coins")).innerText = coins.toString();
    }
}