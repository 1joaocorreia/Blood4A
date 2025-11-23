async function request_clinic_donations(clinic_id) {
    const donations_endpoint = `${rootaddress}/api/info/clinic_donations/${clinic_id}`
    const donations_response = await fetch(donations_endpoint);
    if (! donations_response.ok) {
        return null
    } else {
        return await donations_response.json()
    }
}

async function request_state_donations(state) {
    const donations_endpoint = `${rootaddress}/api/info/state_donations/${state}`
    const donations_response = await fetch(donations_endpoint);
    if (! donations_response.ok) {
        return null
    } else {
        return await donations_response.json()
    }
}