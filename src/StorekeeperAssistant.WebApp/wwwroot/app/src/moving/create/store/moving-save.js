import mutations from "./mutations"
import client from "./Client"

export function saveMoving({ commit }, data) {
    client.createMoving(data)
        .then(p => {
            commit(mutations.setData, { isCreateMoving: false })
            location.reload();
        })
        .catch(e => { commit(mutations.setError, { msg: e }) });
}
