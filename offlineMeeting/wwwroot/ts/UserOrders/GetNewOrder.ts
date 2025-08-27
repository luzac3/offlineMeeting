import { SignalR } from "@root/share/SignalR";

export class GetNewOrder {
    constructor(signalR: SignalR) {
        signalR.get("NewOrder", (data) => {
            const parsedData = <{ [key: string]: string }>data;
            // 読み上げる
            console.log(parsedData.userName);
            location.reload();
        });
    }
}