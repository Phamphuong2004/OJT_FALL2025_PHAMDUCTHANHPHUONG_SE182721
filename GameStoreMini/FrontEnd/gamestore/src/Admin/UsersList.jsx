import React, { useEffect, useState } from "react";
import AdminUserAPI from "../API/AdminUserAPI";

export default function UsersList() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(1);

  const load = async (p = page) => {
    setLoading(true);
    try {
      const data = await AdminUserAPI.list({ page: p, pageSize: 20 });
      // backend may return { data, total, page, pageSize } or an array; handle both
      if (Array.isArray(data)) setUsers(data);
      else if (data && data.data) setUsers(data.data);
      else setUsers([]);
    } catch (e) {
      console.error(e);
      alert("Error loading users: " + (e?.response?.data || e.message));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load(page);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page]);

  const handleChangeRole = async (u) => {
    const role = prompt(`Set role for ${u.email}`, u.role || "Customer");
    if (!role) return;
    try {
      await AdminUserAPI.updateRole(u.id, role);
      alert("Role updated");
      load();
    } catch (e) {
      console.error(e);
      alert("Failed to update role: " + (e?.response?.data || e.message));
    }
  };

  const handleToggleLock = async (u) => {
    try {
      if (u.lockoutEnd) {
        await AdminUserAPI.unlock(u.id);
        alert("User unlocked");
      } else {
        await AdminUserAPI.lock(u.id);
        alert("User locked");
      }
      load();
    } catch (e) {
      console.error(e);
      alert("Failed: " + (e?.response?.data || e.message));
    }
  };

  return (
    <div>
      <h2>Admin — Users</h2>
      {loading ? (
        <div>Loading...</div>
      ) : (
        <table style={{ width: "100%", borderCollapse: "collapse" }}>
          <thead>
            <tr>
              <th>ID</th>
              <th>Email</th>
              <th>Role</th>
              <th>Email confirmed</th>
              <th>Locked</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {users && users.length ? (
              users.map((u) => (
                <tr key={u.id} style={{ borderTop: "1px solid #eee" }}>
                  <td>{u.id}</td>
                  <td>{u.email}</td>
                  <td>{u.role}</td>
                  <td>{String(u.emailConfirmed)}</td>
                  <td>{u.lockoutEnd ? "Yes" : "No"}</td>
                  <td>
                    <button onClick={() => handleChangeRole(u)}>
                      Change role
                    </button>
                    <button
                      onClick={() => handleToggleLock(u)}
                      style={{ marginLeft: 8 }}
                    >
                      {u.lockoutEnd ? "Unlock" : "Lock"}
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={6} style={{ padding: 20 }}>
                  No users found
                </td>
              </tr>
            )}
          </tbody>
        </table>
      )}

      <div style={{ marginTop: 12 }}>
        <button onClick={() => setPage((p) => Math.max(1, p - 1))}>Prev</button>
        <span style={{ margin: "0 8px" }}>Page {page}</span>
        <button onClick={() => setPage((p) => p + 1)}>Next</button>
      </div>
    </div>
  );
}
