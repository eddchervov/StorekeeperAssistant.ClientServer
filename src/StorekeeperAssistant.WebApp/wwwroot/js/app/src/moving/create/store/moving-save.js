import mutations from "./mutations"
import client from "./Client"

export function saveMoving({ commit }, data) {
    client.createMoving(data)
        .then(p => {
            location.reload();
        })
        .catch(e => {
            commit(mutations.setError, {
                msg: e
            })
        });
}
