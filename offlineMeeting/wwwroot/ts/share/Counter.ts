export class Counter {
    SetCounter = () => {
        const counterElements = Array.from(document.getElementsByClassName('counter'));
        counterElements.forEach((element) => {
            const counterElement = <HTMLInputElement>element;

            if (!counterElement) {
                return;
            }
            
            const incrementButtonElement = <HTMLButtonElement>counterElement.querySelector('.counter_increment');
            const decrementButtonElement = <HTMLButtonElement>counterElement.querySelector('.counter_decrement');
            const decrementNumberElement = <HTMLInputElement>counterElement.querySelector('.counter_number');
            const max = parseInt(counterElement.max) || 100;
            const min = parseInt(counterElement.min) || 0;

            let timer: ReturnType<typeof setTimeout>;
            let interval = 200; // 長押し時の間隔(ms)
            let step = 1;
            let longPressStep = 5;

            function changeValue(delta: number) {
                let value = parseInt(decrementNumberElement.value) || 0;
                value += delta;
                value = Math.max(min, Math.min(max, value));
                decrementNumberElement.value = String(value);
            }

            function handlePress(buttonElement: HTMLButtonElement, delta: number) {
                buttonElement.addEventListener('mousedown', function () {
                    step = 1;
                    changeValue(delta);
                    timer = setTimeout(function repeat() {
                        step = longPressStep;
                        changeValue(delta * step);
                        timer = setTimeout(repeat, interval);
                    }, 500); // 0.5秒後に長押し判定
                });
                buttonElement.addEventListener('mouseup', function () {
                    clearTimeout(timer);
                });
                buttonElement.addEventListener('mouseleave', function () {
                    clearTimeout(timer);
                });
                buttonElement.addEventListener('touchend', function () {
                    clearTimeout(timer);
                });
            }

            handlePress(incrementButtonElement, 1);
            handlePress(decrementButtonElement, -1);
        });
    }
} 