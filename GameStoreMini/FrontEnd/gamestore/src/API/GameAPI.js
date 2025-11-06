import { api } from "../API/ApiClient";

function makeAnonId() {
  try {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, (c) =>
      (
        c ^
        (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))
      ).toString(16)
    );
  } catch {
    return "anon-" + Math.random().toString(36).slice(2, 12);
  }
}

api.interceptors.request.use((config) => {
  config.headers = config.headers || {};
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;

  let anon = localStorage.getItem("anonCartId");
  if (!anon) {
    anon = makeAnonId();
    localStorage.setItem("anonCartId", anon);
  }
  config.headers["X-Anonymous-Id"] = anon;

  return config;
});

export async function getAll(params) {
  const res = await api.get("/games", { params });
  return res.data;
}

export async function getOne(id) {
  const res = await api.get(`/games/${id}`);
  return res.data;
}

export async function addGame({
  title,
  description,
  price,
  stock,
  categoryIds,
  imageFile,
  imageUrl,
}) {
  // Backend Create expects JSON (CreateGameDto). We'll post JSON, then if an image file is
  // supplied upload it to the dedicated endpoint and attach returned image URL.
  const payload = {
    Title: title,
    Description: description,
    Price: price,
    Stock: stock,
    CategoryIds: categoryIds,
    ImageUrl: imageUrl,
  };

  const res = await api.post("/games", payload);
  const created = res.data;

  // If an image file was provided, upload it using the dedicated endpoint and merge result
  if ((imageFile && created?.id) || created?.Id || created?.Id === 0) {
    const id = created.Id ?? created.id;
    const fd = new FormData();
    fd.append("file", imageFile);
    const up = await api.post(`/games/${id}/image`, fd, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    // merge imageUrl into created DTO if returned
    if (up?.data?.imageUrl) created.ImageUrl = up.data.imageUrl;
    else if (up?.data?.imageUrl === undefined && up?.data?.imageUrl)
      created.ImageUrl = up.data.imageUrl;
  }

  return created;
}

export async function updateGame(
  id,
  { title, description, price, stock, categoryIds, imageFile, imageUrl }
) {
  const payload = {
    Title: title,
    Description: description,
    Price: price,
    Stock: stock,
    CategoryIds: categoryIds,
    ImageUrl: imageUrl,
  };

  // PUT expects JSON CreateGameDto
  await api.put(`/games/${id}`, payload);

  // If imageFile provided, upload it via image endpoint
  if (imageFile) {
    const fd = new FormData();
    fd.append("file", imageFile);
    const up = await api.post(`/games/${id}/image`, fd, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return up.data;
  }

  return {};
}

export async function deleteGame(id) {
  const res = await api.delete(`/games/${id}`);
  return res.data;
}

export async function getCategories() {
  const res = await api.get("/categories");
  return res.data;
}

export default {
  getAll,
  getOne,
  addGame,
  updateGame,
  deleteGame,
  getCategories,
};
