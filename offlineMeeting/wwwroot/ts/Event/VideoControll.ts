import { SignalR } from "@root/share/SignalR";
import { SetEventListner } from "@root/share/SetEventListner"
import { FetchApi } from "@root/share/FetchApi";
import { CanvasVideo } from "./CanvasVideo"

export class VideoControll {
    cameraContainer: { [key: string]: string }[] = [];
    fps: number = 60;
    intervalId: number = 0;

    url: string;
    method: string;
    headers: { [key: string]: string };
    responseKind: string;

    constructor() {
        this.url = '/Event/GetVideoUsers';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "json";
    }

    controlVideoEvent = async (signalR: SignalR) => {
        // 接続済カメラデバイスを取得、クラスPropertyにIDをセット
        await this.setDevices();

        // カメラ選択optionElementをセット
        const cameraSelectors = document.getElementsByClassName("cameraSelector");
        Array.from(cameraSelectors).forEach(cameraSelector => {
            this.createCameraSelectorObject(cameraSelector);
        });

        // カメラを取得、Optionにセットしたら画面を表示
        document.getElementById("mainContents")!.classList.remove("hidden");

        if (document.getElementById("gameNumber") === null || (<HTMLInputElement>document.getElementById("gameNumber")!).value === "") {
            return;
        }
        const eventNumber = (<HTMLInputElement>document.getElementById("eventNumber")!).value;
        const gameNumber = (<HTMLInputElement>document.getElementById("gameNumber")!).value;

        const fetchApi = new FetchApi();

        const setting = await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            {
                EventNumber: eventNumber,
                GameNumber: gameNumber
            },
            this.responseKind
        ).then(async (data: { [key: string]: string | number }[]) => {
            return data;
        }).catch(e => {
            throw e;
        });

        /*
        const setting = [
            {
                title: "またしても何も知らない",
                name: "大泉洋"
            },
            {
                title: "投げる同人作家",
                name: "佐々木貴賀"
            },
            {
                title: "それでも何もしてくれない",
                name: "荒巻スカルチノフ"
            },
            {
                title: "世界でしか通用しない男",
                name: "小林誠司"
            },
            {
                title: "",
                name: ""
            },
        ];
        */
        let canvasVideoArray: (CanvasVideo | null)[]  = [];

        for (let i = 0; i < 5; i++) {
            canvasVideoArray.push(null);
            // video有効・無効イベントのセット
            SetEventListner.setEvent(
                document.getElementById("listModalWindow"),
                "change",
                "#video" + i + "Checker",
                async event => {
                    this.setVideoInstance(
                        <HTMLInputElement>event.target,
                        canvasVideoArray,
                        <string>setting[i].userTitle,
                        <string>setting[i].userName,
                        i,
                        <number>setting[i].point
                    );
                }
            );
        }

        // canvasVideoArray
        signalR.get("SendEndHandResultEntityList", endHandResultEntityListJson => {
            console.log(endHandResultEntityListJson);
            endHandResultEntityListJson = <{ [key: string]: string | number }[]>endHandResultEntityListJson;
            endHandResultEntityListJson
                .filter(x => x["videoNumber"] != null && canvasVideoArray[<number>x["videoNumber"]] != null)
                .forEach((endHandResultEntity, index) => {
                    this.setUserData(canvasVideoArray[<number>endHandResultEntity["videoNumber"]]!, endHandResultEntity);
                });
        });

        (document.getElementById("video0Button") as HTMLButtonElement).addEventListener(
            "click",
            () => {
                if (canvasVideoArray[0] == null) {
                    return;
                }
                canvasVideoArray[0].myMovingPoint = -48000;
                canvasVideoArray[0].isCaliculate = true;
                canvasVideoArray[0].targetPoint = canvasVideoArray[0].point + canvasVideoArray[0].myMovingPoint;
                canvasVideoArray[0].calcPoint =
                    Math.floor(
                        canvasVideoArray[0].myMovingPoint / (this.fps * 3)
                    );
                canvasVideoArray[0].leftCalcPoint = canvasVideoArray[0].myMovingPoint;
            }
        )

        // videoの表示サイズ変更　厳密にはここじゃない気もする
        SetEventListner.setEvent(
            document.getElementById("listModalWindow"),
            "change",
            "#videoSize",
            async event => {
                this.changeVideoSize();
                // 再生領域全体を確認して、チェックが付いていたら再生し直す
                for (let i = 0; i < 5; i++) {
                    canvasVideoArray.push(null);
                    this.setVideoInstance(
                        (document.getElementById("video" + i + "Checker")! as HTMLInputElement),
                        canvasVideoArray,
                        <string>setting[i].title,
                        <string>setting[i].name,
                        i,
                        <number>setting[i].point
                    );
                }
            }
        )
    }

    private setVideoInstance = async (
        elemnt: HTMLInputElement,
        canvasVideoArray: (CanvasVideo | null)[],
        title: string,
        name: string,
        videoNumber: number,
        point: number
    ) => {
        if (elemnt.checked) {
            // 有効になったCanvasのインスタンス生成
            const canvasVideo = new CanvasVideo(title, name, videoNumber, point);
            canvasVideoArray[videoNumber] = canvasVideo;
            if (this.intervalId) {
                // 一瞬SetIntervalを止めてリフレッシュ
                clearInterval(this.intervalId);
            }
            canvasVideo.activateCanvas();
            await this.setDrawCanvas(canvasVideoArray).then((setInterValId: number) => {
                this.intervalId = setInterValId
            });
            // 有効になったVideoのcvenableをTrueに
            document.getElementById("canvasVideo_" + videoNumber)!.dataset.cvenable = "1";
        } else {
            canvasVideoArray[videoNumber] = null;
            document.getElementById("canvasVideo_" + videoNumber)!.dataset.cvenable = "0";
        }
    }
    
    private setDevices = async () => {
        const devices = (await navigator.mediaDevices.enumerateDevices());
        devices.forEach(device => {
            if (device.kind !== "videoinput") {
                return;
            }
            this.cameraContainer.push(
                {
                    deviceId: device.deviceId,
                    label: device.label
                }
            )
        });
    }

    private createCameraSelectorObject = (selectObject: Element) => {
        this.cameraContainer.forEach((camera) => {
            const cameraOptionElement = document.createElement("option");
            cameraOptionElement.value = camera.deviceId;
            cameraOptionElement.text = camera.label;
            selectObject.appendChild(cameraOptionElement);
        });
    }

    private changeVideoSize = () => {
        const videoSize = (document.getElementById("videoSize") as HTMLInputElement).value;
        const videoWrapperElements = document.getElementsByClassName("videoWrapper");

        // 画面をすべて表示 クラスを一度すべて削除してつけなおし
        Array.from(videoWrapperElements).forEach((element: Element, index: number) => {
            element.classList.remove(...element.classList);
            element.classList.add("videoWrapper");

            switch (videoSize) {
                case "small":
                    element.classList.add("small");
                    break;
                case "filmtsrip":
                    // 一画面を大きく表示して外を並べる
                    element.classList.add("filmtsrip");
                    break;
                case "middle":
                    element.classList.add("middle");

                    // 4画面目までは削除しない
                    if (index < 4) {
                            return;
                    }

                    // videoを停止して非表示
                    (document.getElementById("video" + index + "Checker") as HTMLInputElement).checked = false;
                    element.classList.add("nodisplay");
                    break;
                case "large":
                    element.classList.add("large");
                    // 1画面目は残す
                    if (index < 1) {
                        return;
                    }

                    // videoを停止して非表示
                    (document.getElementById("video" + index + "Checker") as HTMLInputElement).checked = false;
                    element.classList.add("nodisplay");
                    break;
            }
        });
    }

    private setDrawCanvas = async (canvasVideoArray: (CanvasVideo | null)[]) => {
        return await window.setInterval(() => {
            canvasVideoArray.forEach(canvasVideo => {
                if (canvasVideo !== null){
                    canvasVideo.drawCanvas();
                }
            });
        }, 1000 / this.fps);
    }

    private setUserData = (canvasVideo: CanvasVideo, endHandResultEntity: {[key:string]: string | number}) => {
        setTimeout(() => {
            canvasVideo.myMovingPoint = <number>endHandResultEntity["movingPoint"];
            canvasVideo.targetPoint = <number>endHandResultEntity["point"];
            canvasVideo.isCaliculate = true;
            canvasVideo.calcPoint = Math.floor(
                canvasVideo.myMovingPoint / 60 * 5
            );
            canvasVideo.leftCalcPoint = canvasVideo.myMovingPoint;
            canvasVideo.resultCd = <string>endHandResultEntity["resultCd"];
        }, 3000);
    }
}