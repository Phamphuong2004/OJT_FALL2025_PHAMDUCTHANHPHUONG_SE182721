# Hệ thống Review - Hướng dẫn sử dụng

## 📦 Các Component đã tạo:

### 1. **ReviewList.jsx** - Component chính
Hiển thị toàn bộ hệ thống review cho một game

```jsx
import ReviewList from './Review/ReviewList';

// Trong trang GameDetails
<ReviewList gameId={gameId} />
```

### 2. **ReviewSummary.jsx** - Component tóm tắt
Hiển thị rating ngắn gọn (dùng trong GameCard)

```jsx
import ReviewSummary from './Review/ReviewSummary';

// Trong GameCard
<ReviewSummary 
  averageRating={game.averageRating} 
  totalReviews={game.reviewCount} 
/>
```

### 3. **Các Component phụ:**
- `StarRating.jsx` - Hiển thị sao
- `ReviewStats.jsx` - Thống kê rating
- `ReviewFilter.jsx` - Bộ lọc
- `ReviewForm.jsx` - Form tạo/sửa review
- `ReviewItem.jsx` - Một review item
- `useReviews.js` - Custom hook quản lý state

## 🎯 Cách tích hợp:

### 1. Trong trang GameDetails:

```jsx
import React from 'react';
import ReviewList from '../Review/ReviewList';

const GameDetails = ({ gameId }) => {
  return (
    <div className="game-details">
      {/* Game info */}
      <div className="game-info">
        {/* ... game details ... */}
      </div>

      {/* Reviews section */}
      <div className="reviews-section">
        <ReviewList gameId={gameId} />
      </div>
    </div>
  );
};
```

### 2. Trong GameCard (danh sách games):

```jsx
import React from 'react';
import ReviewSummary from '../Review/ReviewSummary';

const GameCard = ({ game }) => {
  return (
    <div className="game-card">
      <img src={game.imageUrl} alt={game.title} />
      <h3>{game.title}</h3>
      <p>{game.price}đ</p>
      
      {/* Add review summary */}
      <ReviewSummary 
        averageRating={game.averageRating} 
        totalReviews={game.reviewCount} 
      />
    </div>
  );
};
```

### 3. Update Model Game (nếu chưa có):

Backend đã có `AverageRating` và `ReviewCount`, frontend cần map đúng:

```javascript
// Khi fetch games từ API
const games = response.data.map(game => ({
  ...game,
  averageRating: game.averageRating || 0,
  reviewCount: game.reviewCount || 0
}));
```

## 🔐 Phân quyền:

| Chức năng | Guest | Customer | Admin |
|-----------|-------|----------|-------|
| Xem reviews | ✅ | ✅ | ✅ |
| Viết review | ❌ | ✅ (đã mua) | ✅ |
| Sửa review của mình | ❌ | ✅ | ✅ |
| Xóa review của mình | ❌ | ✅ | ✅ |
| Vote helpful | ❌ | ✅ | ✅ |
| Xóa review người khác | ❌ | ❌ | ✅ |

## 🎨 Tính năng:

✅ Đánh giá từ 1-5 sao  
✅ Viết comment (tối đa 1000 ký tự)  
✅ Chỉ người đã mua mới review được  
✅ Mỗi user chỉ review 1 lần cho 1 game  
✅ Vote "Helpful" cho review  
✅ Thống kê rating (trung bình, phân bố sao)  
✅ Lọc reviews theo nhiều tiêu chí  
✅ Sắp xếp theo date/rating/helpful  
✅ Phân trang  
✅ Verified purchase badge  
✅ Chỉnh sửa/xóa review của mình  
✅ Admin có thể xóa bất kỳ review nào  

## 📝 Lưu ý:

1. **Token Authentication**: Đảm bảo token được lưu trong localStorage với key `'token'`
2. **API URL**: Cấu hình `VITE_API_URL` trong file `.env`
3. **Pagination Component**: Sử dụng component có sẵn `../Components/Pagination`
4. **User Role**: Sử dụng `getUserRole()` từ `../Auth/useAuth` để kiểm tra quyền

## 🚀 Testing:

1. **Guest user**: Chỉ xem được reviews
2. **Customer**: Đăng nhập → Mua game → Viết review
3. **Admin**: Có thể xóa bất kỳ review nào

## 📂 File Structure:

```
src/
├── Review/
│   ├── ReviewList.jsx          ✅
│   ├── ReviewList.css          ✅
│   ├── ReviewSummary.jsx       ✅
│   ├── ReviewSummary.css       ✅
│   ├── ReviewItem.jsx          ✅
│   ├── ReviewItem.css          ✅
│   ├── ReviewForm.jsx          ✅
│   ├── ReviewForm.css          ✅
│   ├── ReviewStats.jsx         ✅
│   ├── ReviewStats.css         ✅
│   ├── ReviewFilter.jsx        ✅
│   ├── ReviewFilter.css        ✅
│   ├── StarRating.jsx          ✅
│   ├── StarRating.css          ✅
│   └── useReviews.js           ✅
├── API/
│   └── ReviewAPI.js            ✅
```

## 🎯 Next Steps:

1. Tích hợp `ReviewList` vào trang GameDetails
2. Tích hợp `ReviewSummary` vào GameCard
3. Test với các vai trò khác nhau
4. Thêm notifications khi có action thành công/thất bại (có thể dùng Toast component có sẵn)

## 💡 Ví dụ sử dụng Toast (nếu có):

```jsx
import { useToast } from '../Components/Toast';

const MyComponent = () => {
  const { showToast } = useToast();

  const handleSuccess = () => {
    showToast('Đánh giá của bạn đã được gửi!', 'success');
  };

  const handleError = () => {
    showToast('Có lỗi xảy ra!', 'error');
  };
};
```
