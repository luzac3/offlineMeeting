import { SetEventListner } from "@root/share/SetEventListner"

export class SetMikeDevice {
    mikeContainer: { [key: string]: string }[] = [];

    setGetMikeEvent = () => {
        SetEventListner.setEvent(
            document.getElementById("listModalWindow"),
            "click",
            "#getMikeDevice",
            (event) => {
                this.getMike();
            }
        )
    }

    private getMike = async () => {
        // 接続済マイクデバイスを取得、クラスPropertyにIDをセット
        await this.setDevices();

        // マイク選択optionElementをセット
        const audioSelector = document.getElementById("audioSelector") as HTMLInputElement;
        this.createMikeSelectorObject(audioSelector);
    }

    private setDevices = async () => {
        navigator.mediaDevices.getUserMedia({ video: true, audio: true });
        const devices = await navigator.mediaDevices.enumerateDevices();
        this.mikeContainer = devices
            .filter(device => device.kind == "audioinput")
            .map((v, i) => ({ deviceId: v.deviceId, label: v.label }));
    }

    private createMikeSelectorObject = (selectObject: Element) => {
        this.mikeContainer.forEach((mike) => {
            const mikeOptionElement = document.createElement("option");
            mikeOptionElement.value = mike.deviceId;
            mikeOptionElement.text = mike.label;
            selectObject.appendChild(mikeOptionElement);
        });
    }
}