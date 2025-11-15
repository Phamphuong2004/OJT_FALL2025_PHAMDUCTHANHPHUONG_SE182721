import React, { useEffect, useState } from "react";
import { useAuth } from "../Auth/useAuth";
import UserAPI from "../API/UserAPI";
import Toast from "../Components/Toast";

const UserProfile = () => {
  const { user, updateUser } = useAuth();
  const [activeTab, setActiveTab] = useState("profile");
  const [isEditing, setIsEditing] = useState(false);
  const [loading, setLoading] = useState(false);
  const [toast, setToast] = useState({ show: false, message: "", type: "" });

  const [profileData, setProfileData] = useState({
    username: "",
    email: "",
    fullName: "",
    phoneNumber: "",
    dateOfBirth: "",
    gender: "",
    avatar: "",
  });

  const [passwordData, setPasswordData] = useState({
    currentPassword: "",
    newPassword: "",
    confirmPassword: "",
  });

  const [avatarPreview, setAvatarPreview] = useState(null);

  useEffect(() => {
    if (user) {
      setProfileData({
        username: user.username || "",
        email: user.email || "",
        fullName: user.fullName || "",
        phoneNumber: user.phoneNumber || "",
        dateOfBirth: user.dateOfBirth?.split("T")[0] || "",
        gender: user.gender || "",
        avatar: user.avatar || "",
      });
      setAvatarPreview(user.avatar || null);
    }
  }, [user]);

  const showToast = (message, type) => {
    setToast({ show: true, message, type });
    setTimeout(() => setToast({ show: false, message: "", type: "" }), 3000);
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setProfileData((prevData) => ({ ...prevData, [name]: value }));
  };

  const handlePasswordChange = (e) => {
    const { name, value } = e.target;
    setPasswordData((prevData) => ({ ...prevData, [name]: value }));
  };

  const handleAvatarChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      if (file.size > 2 * 1024 * 1024) {
        showToast("Avatar size should be less than 2MB", "error");
        return;
      }

      const reader = new FileReader();
      reader.onloadend = () => {
        setAvatarPreview(reader.result);
        setProfileData((prevData) => ({ ...prevData, avatar: reader.result }));
      };
      reader.readAsDataURL(file);
    }
  };

  const handleProfileUpdate = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      const response = await UserAPI.updateProfile(profileData);
      updateUser(response);
      setIsEditing(false);
      showToast("Profile updated successfully", "success");
    } catch (err) {
      showToast(
        err.response?.data?.message || "Failed to update profile",
        "error"
      );
    } finally {
      setLoading(false);
    }
  };

  const handlePasswordUpdate = async (e) => {
    e.preventDefault();

    if (passwordData.newPassword !== passwordData.confirmPassword) {
      showToast("New passwords do not match", "error");
      return;
    }

    if (passwordData.newPassword.length < 6) {
      showToast("New password must be at least 6 characters long", "error");
      return;
    }

    setLoading(true);
    try {
      await UserAPI.changePassword({
        currentPassword: passwordData.currentPassword,
        newPassword: passwordData.newPassword,
      });
      setPasswordData({
        currentPassword: "",
        newPassword: "",
        confirmPassword: "",
      });
      showToast("Password changed successfully", "success");
    } catch (err) {
      showToast(
        err.response?.data?.message || "Failed to change password",
        "error"
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      {toast.show && <Toast message={toast.message} type={toast.type} />}

      <div>
        {/* Sidebar */}
        <div>
          <div>
            <img
              src={avatarPreview || "/default-avatar.png"}
              alt="Avatar"
              width="100"
            />
            <div>{user?.username}</div>
          </div>

          <nav>
            <button onClick={() => setActiveTab("profile")}>My Profile</button>
            <button onClick={() => setActiveTab("password")}>
              Change Password
            </button>
            <button onClick={() => setActiveTab("addresses")}>
              My Addresses
            </button>
            <button onClick={() => setActiveTab("orders")}>My Orders</button>
          </nav>
        </div>

        {/* Content */}
        <div>
          {activeTab === "profile" && (
            <div>
              <h2>My Profile</h2>
              <p>Manage your profile information</p>

              <form onSubmit={handleProfileUpdate}>
                <div>
                  <label>Username</label>
                  <input
                    type="text"
                    name="username"
                    value={profileData.username}
                    disabled
                  />
                </div>

                <div>
                  <label>Email</label>
                  <input
                    type="email"
                    name="email"
                    value={profileData.email}
                    onChange={handleInputChange}
                    disabled={!isEditing}
                  />
                </div>

                <div>
                  <label>Full Name</label>
                  <input
                    type="text"
                    name="fullName"
                    value={profileData.fullName}
                    onChange={handleInputChange}
                    disabled={!isEditing}
                    placeholder="Enter your full name"
                  />
                </div>

                <div>
                  <label>Phone Number</label>
                  <input
                    type="tel"
                    name="phoneNumber"
                    value={profileData.phoneNumber}
                    onChange={handleInputChange}
                    disabled={!isEditing}
                    placeholder="Enter your phone number"
                  />
                </div>

                <div>
                  <label>Date of Birth</label>
                  <input
                    type="date"
                    name="dateOfBirth"
                    value={profileData.dateOfBirth}
                    onChange={handleInputChange}
                    disabled={!isEditing}
                  />
                </div>

                <div>
                  <label>Gender</label>
                  <select
                    name="gender"
                    value={profileData.gender}
                    onChange={handleInputChange}
                    disabled={!isEditing}
                  >
                    <option value="">Select gender</option>
                    <option value="Male">Male</option>
                    <option value="Female">Female</option>
                    <option value="Other">Other</option>
                  </select>
                </div>

                {isEditing && (
                  <div>
                    <label>Avatar</label>
                    <div>
                      <img
                        src={avatarPreview || "/default-avatar.png"}
                        alt="Preview"
                        width="80"
                      />
                      <input
                        type="file"
                        accept="image/*"
                        onChange={handleAvatarChange}
                        id="avatar-upload"
                      />
                    </div>
                    <p>Max file size: 2MB</p>
                  </div>
                )}

                <div>
                  {!isEditing ? (
                    <button type="button" onClick={() => setIsEditing(true)}>
                      Edit Profile
                    </button>
                  ) : (
                    <>
                      <button
                        type="button"
                        onClick={() => {
                          setIsEditing(false);
                          setProfileData({
                            username: user.username || "",
                            email: user.email || "",
                            fullName: user.fullName || "",
                            phoneNumber: user.phoneNumber || "",
                            dateOfBirth: user.dateOfBirth?.split("T")[0] || "",
                            gender: user.gender || "",
                            avatar: user.avatar || "",
                          });
                          setAvatarPreview(user.avatar);
                        }}
                        disabled={loading}
                      >
                        Cancel
                      </button>
                      <button type="submit" disabled={loading}>
                        {loading ? "Saving..." : "Save Changes"}
                      </button>
                    </>
                  )}
                </div>
              </form>
            </div>
          )}

          {activeTab === "password" && (
            <div>
              <h2>Change Password</h2>
              <p>Ensure your account security</p>

              <form onSubmit={handlePasswordUpdate}>
                <div>
                  <label>Current Password</label>
                  <input
                    type="password"
                    name="currentPassword"
                    value={passwordData.currentPassword}
                    onChange={handlePasswordChange}
                    required
                    placeholder="Enter current password"
                  />
                </div>

                <div>
                  <label>New Password</label>
                  <input
                    type="password"
                    name="newPassword"
                    value={passwordData.newPassword}
                    onChange={handlePasswordChange}
                    required
                    placeholder="Enter new password (min 6 characters)"
                  />
                </div>

                <div>
                  <label>Confirm New Password</label>
                  <input
                    type="password"
                    name="confirmPassword"
                    value={passwordData.confirmPassword}
                    onChange={handlePasswordChange}
                    required
                    placeholder="Confirm new password"
                  />
                </div>

                <div>
                  <button type="submit" disabled={loading}>
                    {loading ? "Changing..." : "Change Password"}
                  </button>
                </div>
              </form>
            </div>
          )}

          {activeTab === "addresses" && (
            <div>
              <h2>My Addresses</h2>
              <p>Manage your delivery addresses</p>
              <p>This feature will use the existing AddressManager component</p>
            </div>
          )}

          {activeTab === "orders" && (
            <div>
              <h2>My Orders</h2>
              <p>Track and manage your orders</p>
              <p>This feature will use the existing OrderTracking component</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default UserProfile;
