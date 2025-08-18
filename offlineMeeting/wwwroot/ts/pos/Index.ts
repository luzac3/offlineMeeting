import { SignalR } from "@root/share/SignalR";
import { CaliculateCoinNumber } from "./CaliculateCoinNumber";
import { StockOrder } from "./StockOrder";


(async () => {
    const signalR = new SignalR();
    const caliculateCoinNumber = new CaliculateCoinNumber();
    const stockOrder = new StockOrder();

    caliculateCoinNumber.setCoin();
    stockOrder.addOrder();
})();