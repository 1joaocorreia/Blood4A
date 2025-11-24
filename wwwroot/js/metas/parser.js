function parse_condition(condition, json_data) {

	return condition.replace(/donations\[(\d+)\]/gi, (match, capture) => {
	
		const idx = parseInt(capture, 10);   // número dentro dos colchetes
		if (isNaN(idx) || idx < 1 || idx > 12) {
			return match;
		}

		const item = json_data?.doacoesPorMes?.[idx - 1];
		if (!item) {
			return 'null';
		}

		return String(item.quantidadeDeDoacoes);
	});
}
