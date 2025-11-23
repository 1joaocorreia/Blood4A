async function request_clinic_goals(clinic_id) {
    const goals_endpoint = `${rootaddress}/api/info/clinic_goals/${clinic_id}`
    const goals_response = await fetch(goals_endpoint);
    if (! goals_response.ok) {
        return null
    } else {
        return await goals_response.json()
    }
}

async function request_state_goals(state) {
    const goals_endpoint = `${rootaddress}/api/info/state_goals/${state}`
    const goals_response = await fetch(goals_endpoint);
    if (! goals_response.ok) {
        return null
    } else {
        return await goals_response.json()
    }
}