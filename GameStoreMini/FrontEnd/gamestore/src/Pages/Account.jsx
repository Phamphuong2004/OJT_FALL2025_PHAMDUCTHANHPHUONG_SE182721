import React, { useEffect, useState } from "react";
import { getProfile, updateProfile } from "../API/UserAPI";
import { isAuthenticated } from "../Auth/useAuth";
import { useNavigate } from "react-router-dom";

export default function Account() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [profile, setProfile] = useState(null);
  const [editing, setEditing] = useState(false);
  const [saving, setSaving] = useState(false);
  const [form, setForm] = useState({ fullName: "", phoneNumber: "" });

  useEffect(() => {
    let mounted = true;
    async function load() {
      setLoading(true);
      setError(null);
      try {
        if (!isAuthenticated()) {
          // redirect to login if not authenticated
          navigate("/login");
          return;
        }

        const data = await getProfile();
        if (!mounted) return;

        // normalize fields from server (handle different casing)
        const normalized = {
          email: data.email || data.Email || data.userName || "",
          role: data.role || data.Role || null,
          fullName:
            data.fullName || data.FullName || data.name || data.Name || "",
          phoneNumber: data.phoneNumber || data.PhoneNumber || data.phone || "",
        };

        setProfile(normalized);
        setForm({
          fullName: normalized.fullName,
          phoneNumber: normalized.phoneNumber,
        });
      } catch (err) {
        setError("Không thể tải thông tin người dùng.");
        console.warn(err);
      } finally {
        setLoading(false);
      }
    }
    load();
    return () => (mounted = false);
  }, []);

  async function onSave(e) {
    e.preventDefault();
    setError(null);
    if (!form.fullName || form.fullName.trim() === "") {
      setError("Tên không được để trống.");
      return;
    }
    setSaving(true);
    try {
      // payload keys follow backend convention (FullName, PhoneNumber)
      const res = await updateProfile({
        FullName: form.fullName,
        PhoneNumber: form.phoneNumber || null,
      });

      // some APIs return updated object directly, some wrap in data
      const updated =
        res && (res.fullName || res.FullName || res.email)
          ? res
          : await getProfile();

      const normalized = {
        email: updated.email || updated.Email || profile?.email || "",
        role: updated.role || updated.Role || profile?.role || null,
        fullName:
          updated.fullName ||
          updated.FullName ||
          updated.name ||
          updated.Name ||
          form.fullName,
        phoneNumber:
          updated.phoneNumber ||
          updated.PhoneNumber ||
          updated.phone ||
          form.phoneNumber ||
          "",
      };

      setProfile(normalized);
      try {
        localStorage.setItem("userProfile", JSON.stringify(normalized));
        window.dispatchEvent(new Event("authChanged"));
      } catch (e) {}
      setEditing(false);
    } catch (err) {
      console.error(err);
      setError("Cập nhật thất bại. Vui lòng thử lại.");
    } finally {
      setSaving(false);
    }
  }

  if (loading) return <div style={{ padding: 20 }}>Đang tải...</div>;
  if (error)
    return (
      <div style={{ padding: 20 }}>
        <div style={{ color: "#b00", marginBottom: 12 }}>{error}</div>
      </div>
    );

  return (
    <div style={{ padding: 20, maxWidth: 900 }}>
      <h2>Thông tin tài khoản</h2>
      <div style={{ marginTop: 12 }}>
        <div>
          <b>Email:</b> {profile?.email}
        </div>
        <div>
          <b>Vai trò:</b> {profile?.role || "Customer"}
        </div>
        <div style={{ marginTop: 12 }}>
          <b>Tên đầy đủ:</b>
          {!editing ? (
            <span style={{ marginLeft: 8 }}>
              {profile?.fullName || profile?.fullName || "-"}
            </span>
          ) : (
            <input
              value={form.fullName}
              onChange={(e) =>
                setForm((f) => ({ ...f, fullName: e.target.value }))
              }
              style={{ marginLeft: 8 }}
            />
          )}
        </div>
        <div style={{ marginTop: 8 }}>
          <b>Số điện thoại:</b>
          {!editing ? (
            <span style={{ marginLeft: 8 }}>{profile?.phoneNumber || "-"}</span>
          ) : (
            <input
              value={form.phoneNumber}
              onChange={(e) =>
                setForm((f) => ({ ...f, phoneNumber: e.target.value }))
              }
              style={{ marginLeft: 8 }}
            />
          )}
        </div>

        <div style={{ marginTop: 16 }}>
          {!editing ? (
            <button onClick={() => setEditing(true)}>Chỉnh sửa</button>
          ) : (
            <>
              <button onClick={onSave} style={{ marginRight: 8 }}>
                Lưu
              </button>
              <button onClick={() => setEditing(false)}>Huỷ</button>
            </>
          )}
        </div>
      </div>
    </div>
  );
}
