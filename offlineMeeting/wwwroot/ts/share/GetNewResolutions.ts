export class GetNewResolutions {
    static Get = (
        canvasWrapperElement: HTMLElement,
        resolution: { [key: string]: number },
        aspectRatioKind: string
    ) => {
        let dataSet: { [key: string]: { [key: string]: number } } = {};
        switch (aspectRatioKind) {
            case "VGA":
                dataSet = {
                    characterFrame: { x: 0, y: 400, w: 800, h: 210 },
                    leftUp: { x: 0, y: 0, w: 0, h: 0 },
                    field: { x: 0, y: 0, font: 0 },
                    leachImage: { x: 0, y: 0, w: 0, h: 0 },
                    leach: { x: 0, y: 0, font: 0 },
                    honbaImage: { x: 0, y: 0, w: 0, h: 0 },
                    honba: { x: 0, y: 0, font: 0 },
                    face: { x: 80, y: 420, w: 150, h: 170 },
                    point: { x: 780, y: 575, font: 75 },

                    title: { x: 780, y: 445, font: 30 },
                    name: { x: 780, y: 505, font: 60 },

                    yaku: { x: 0, y: 0, font: 0 },
                    kind: { x: 750, y: 385, font: 60 },
                    movingPoint: { x: 780, y: 465, font: 30 },
                    allow: { x: 700, y: 505, font: 45 },
                }
                break;
            case "HDTV":
                dataSet = {
                    characterFrame: { x: 0, y: 620, w: 1280, h: 300 },
                    leftUp: { x: 0, y: 0, w: 0, h: 0 },
                    field: { x: 0, y: 0, font: 0 },
                    leachImage: { x: 0, y: 0, w: 0, h: 0 },
                    leach: { x: 0, y: 0, font: 0 },
                    honbaImage: { x: 0, y: 0, w: 0, h: 0 },
                    honba: { x: 0, y: 0, font: 0 },
                    face: { x: 660, y: 650, w: 160, h: 180 },
                    point: { x: 1245, y: 825, font: 75 },

                    name: { x: 1245, y: 755, font: 60 },
                    title: { x: 1245, y: 695, font: 30 },

                    allow: { x: 1255, y: 755, font: 45 },
                    movingPoint: { x: 1245, y: 520, font: 30 },
                    yaku: { x: 0, y: 0, font: 0 },
                    kind: { x: 1230, y: 755, font: 60 },
                }
        }

        let widthRate = canvasWrapperElement.clientWidth / resolution.w;
        let heightRate = canvasWrapperElement.clientHeight / resolution.h;

        const canvasWidth = widthRate < heightRate ?
            canvasWrapperElement.clientWidth :
            canvasWrapperElement.clientHeight / resolution.h * resolution.w;
        const canvasHeight = heightRate < widthRate ?
            canvasWrapperElement.clientHeight :
            canvasWrapperElement.clientWidth / resolution.w * resolution.h;

        let fontRate = widthRate < heightRate ? widthRate : heightRate;

        const canvasXFrame = Math.floor((canvasWidth - (resolution.w * widthRate)) / 2);
        const canvasYFrame = Math.floor((canvasHeight - (resolution.h * heightRate)) / 2);

        // widthチェック
        Object.keys(dataSet).forEach((key: string) => {
            dataSet[key].x = Math.floor(dataSet[key].x * widthRate) + canvasXFrame;
            dataSet[key].y = Math.floor(dataSet[key].y * heightRate) + canvasYFrame;

            if ("w" in dataSet[key]) {
                dataSet[key].w = Math.floor(dataSet[key].w * widthRate);
                dataSet[key].h = Math.floor(dataSet[key].h * heightRate);
            } else {
                dataSet[key].font = Math.floor(dataSet[key].font * fontRate);
            }
        });

        dataSet.canvas = { w: canvasWidth, h: canvasHeight };
        dataSet.video = {
            x: canvasXFrame,
            y: canvasYFrame,
            w: Math.floor(resolution.w * widthRate),
            h: Math.floor(resolution.h * heightRate)
        };

        return dataSet;
    }
}