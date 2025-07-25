import Hls from "hls.js";
import { SetEventListner } from "@root/share/SetEventListner";
import { SignalR } from "@root/share/SignalR";
import { SetCanvas } from "./SetCanvas";
import { VideoSwitcher } from "./VideoSwitcher";

export class ManagementVideo {
    EventNumber: string;
    GameNumber: string;
    HandNumber: string;
    HandSubNumber: string;

    constructor() {
        this.EventNumber = (<HTMLInputElement>document.getElementById("eventNumber")).value;
        this.GameNumber = (<HTMLInputElement>document.getElementById("gameNumber")).value;
        this.HandNumber = (<HTMLInputElement>document.getElementById("handNumber")).value;
        this.HandSubNumber = (<HTMLInputElement>document.getElementById("handSubNumber")).value;
    }

    setStreamingEvent = (signalR: SignalR) => {
        SetEventListner.setEvent(
            document.getElementById("listModalWindow"),
            "click",
            "#checkVideoButton",
            () => {
                this.activateVideo(signalR);
            },
            true
        );
    }

    private activateVideo = (signalR: SignalR) => {
        // eventの再登録をし、SignalR接続をスキップして再生するようにする
        SetEventListner.setEvent(
            document.getElementById("listModalWindow"),
            "click",
            "#checkVideoButton",
            () => {
                this.playVideos();
            }
        );
        (<HTMLElement>document.getElementById("checkVideoButton")).innerText = "映像再生";


        signalR.get("SendMediaProperty", (json) => {
            json = json as  { [key: string]: string | number | boolean }
            const video = document.getElementById("video_" + json["mediaNumber"]) as HTMLVideoElement;
            const videoSrc = json["m3u8FilePath"] as string;

            const userCdElement = video.closest(".videoWrapper")?.querySelector(".userCd") as HTMLElement;
            const firstDirectionElement = video.closest(".videoWrapper")?.querySelector(".firstDirection") as HTMLElement;
            const directionElement = video.closest(".videoWrapper")?.querySelector(".direction") as HTMLElement;

            userCdElement.dataset.usercd = json["userCd"] as string;
            userCdElement.innerText = json["userName"] as string;

            firstDirectionElement.dataset.firstdirection = json["firstDirection"] as string;

            directionElement.dataset.direction = json["direction"] as string;
            switch (json["direction"].toString()) {
                case "1":
                    directionElement.innerText = "東";
                    break;
                case "2":
                    directionElement.innerText = "南";
                    break;
                case "3":
                    directionElement.innerText = "西";
                    break;
                case "4":
                    userCdElement.innerText = "北";
                    break;
            }

            console.log(json);
            // データが入ったら並び替えを開始　ソートはfirstDirection
            this.sortVideoELement();

            this.showVideo(video, videoSrc);
        });

        for (let i = 0; i < 4; i++) {
            // Video番号ごとにsignalR受信ファイル呼び出し
            signalR.send(
                "GetIndex",
                Number(this.EventNumber),
                Number(this.GameNumber),
                Number(this.HandNumber),
                Number(this.HandSubNumber),
                "management/",
                true,
                i
            );
        }

        // 動画の表示ボタンを押したらcanvasの描画を開始する
        const setCanvas = new SetCanvas();
        const videoSwitcher = new VideoSwitcher();
        setCanvas.setDrawCanvas();
        videoSwitcher.setSwitchEvent(setCanvas);
    };

    private playVideos = () => {
        const videoElements = document.getElementsByClassName("videoWrapper") as HTMLCollectionOf<HTMLElement>;
        // 再生時にVideoコンテンツを取得して一番大きい時間を取得し、全てそれに合わせる
        const firstVideoCurrentTime = Array.from(videoElements)
            .map((v, i) => (<HTMLVideoElement>v.querySelector("video")).currentTime)
            .sort((x, y) => y - x)[0]

        Array.from(videoElements).forEach((videoElement: HTMLElement) => {
            let streaming = videoElement.querySelector("video") as HTMLVideoElement;
            streaming.currentTime = firstVideoCurrentTime;
            streaming.play();
        });
    }

    private showVideo = (videoElement: HTMLVideoElement, videoSrc: string) => {
        if (Hls.isSupported()) {
            var hls = new Hls();
            hls.loadSource(videoSrc);
            hls.attachMedia(videoElement);
        }
        else if (videoElement.canPlayType('application/vnd.apple.mpegurl')) {
            videoElement.src = videoSrc;
        }

        this.playVideos();
    }

    private sortVideoELement = () => {
        const video = document.getElementById("video") as HTMLElement;
        const videoElements = document.getElementsByClassName("videoWrapper") as HTMLCollectionOf<HTMLElement>;

        const test = Array.from(videoElements).sort(
            (x, y) => {
                var a = (<HTMLElement>x.querySelector(".firstDirection")).dataset.firstdirection as string;
                var b = (<HTMLElement>y.querySelector(".firstDirection")).dataset.firstdirection as string;

                return parseInt(a) - parseInt(b);
            }
        );

        Array.from(videoElements).sort(
            (x, y) => {
                var a = (<HTMLElement>x.querySelector(".firstDirection")).dataset.firstdirection as string;
                var b = (<HTMLElement>y.querySelector(".firstDirection")).dataset.firstdirection as string;

                return parseInt(a) - parseInt(b);
            }
        ).forEach((videoElement: HTMLElement) => {
            // 削除してから改めて追加
            videoElement.remove();
            video.appendChild(videoElement);
        });
    }
}