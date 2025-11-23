function parse_condition(condition, json_data) {

    let lcondition = condition.toLowerCase();
    // x > x
    // donations[2] > donations[5]
    // ...

    // replacing donations[x] for its respective value
    last_ocurrence = 0
    preventive_loop = 0
    while (true){
        preventive_loop++   
        if (preventive_loop >= 5) {
            break
        }
        // looking for the location of multiple "donation"
        last_ocurrence = lcondition.indexOf("donation", last_ocurrence)
        if (last_ocurrence == -1) {
            break
        }
        console.log("Last ocurrence: " + last_ocurrence)

        // extracting the index
        let start_bracket
        let end_bracket
        
        start_bracket = lcondition.indexOf('[', last_ocurrence)
        end_bracket = lcondition.indexOf(']', last_ocurrence)
        console.log("Start bracket: " + start_bracket)
        console.log("End bracket: " + end_bracket)

        if (start_bracket == -1 || end_bracket == -1) {
            break
        }

        const index = lcondition.slice(start_bracket + 1, end_bracket).trim()
        if (index < 1 || index > 12) {
            break
        }
        console.log("Index: " + index)
        // replacing the entire expression
        const data = json_data.doacoesPorMes[index-1].quantidadeDeDoacoes
        lcondition = lcondition.replace(`${lcondition.slice(last_ocurrence, start_bracket)}[${index}]`, data)
        
        last_ocurrence = last_ocurrence + 8
        continue;

    }

    // returning the value
    return lcondition

}