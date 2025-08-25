import { SendCoins } from "./SendCoins";

declare const paypal: any;

export class CreatePaypalButton {
    SetPaypalButton = () => {
        if (document.getElementsByClassName('purcahse_coins').length === 0) {
            return;
        }
        this.Create(120, 1, '#paypal_button_120');
        this.Create(600, 6, '#paypal_button_600');
        this.Create(1200, 13, '#paypal_button_1200');
        this.Create(3600, 40, '#paypal_button_3600');
        this.Create(6000, 68, '#paypal_button_6000');
        this.Create(12000, 140, '#paypal_button_12000');
    }

    private Create = (value: number, coins:number,  buttonContainer: string) => {
        paypal.Buttons({
            createOrder: (data: any, actions: any) => {
                // 支払い金額などを指定
                return actions.order.create({
                    purchase_units: [{
                        amount: {
                            value: value
                        }
                    }]
                });
            },
            onApprove:  (data: any, actions: any) => {
                return actions.order.capture().then((details: any) => {
                    const sendCoins = new SendCoins();
                    sendCoins.setCoinsData(value, coins, details);
                });
            }
        }).render(buttonContainer);
    }
}