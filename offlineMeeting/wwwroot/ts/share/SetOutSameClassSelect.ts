export class SetOutSameClassSelect {
	setSelectorEvent = (targets: HTMLCollectionOf<HTMLSelectElement>) => {
		this.setSelectorEventOnce(targets);
		Array.from(targets).forEach((element: HTMLSelectElement) => {
			// fixme
			element.addEventListener("change", (event) => {
				const changedTarget = <HTMLSelectElement>event.target;
				this.setSelectorEventOnce(targets, changedTarget);
			});
		});
	}

	setSelectorEventOnce = (
		targets: HTMLCollectionOf<HTMLSelectElement>,
		changedTarget: HTMLSelectElement | null = null
	) => {
		const alreadySelectedValueArray: string[] = Array.from(targets).map(target => target.value);
		changedTarget = changedTarget ?? targets.item(0);

		Array.from(targets).forEach((element: HTMLSelectElement) => {
			alreadySelectedValueArray.shift();
			let thisValue = element.value
			if (
				element !== changedTarget && 
				alreadySelectedValueArray.includes(element.value)
			) {
				thisValue = this.deduplicationOption(alreadySelectedValueArray, element);
			}
			alreadySelectedValueArray.push(thisValue);
		});
	}

	private deduplicationOption = (alreadySelectedValueArray: string[], element: HTMLSelectElement) => {
		const optionElements = element.children as HTMLCollectionOf<HTMLOptionElement>;

		for (let optionElement of Array.from(optionElements)) {
			if (!alreadySelectedValueArray.includes(optionElement.value)) {
				optionElement.selected = true;
				return optionElement.value;
			}
		}
		throw new Error("element anomaly");
	}
}