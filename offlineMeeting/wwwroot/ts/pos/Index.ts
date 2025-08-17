import { SignalR } from "@root/share/SignalR";
import { CaliculateCoinNumber } from "./CaliculateCoinNumber";

(async () => {
    const signalR = new SignalR();
    const caliculateCoinNumber = new CaliculateCoinNumber();

    caliculateCoinNumber.setCoin();
})();