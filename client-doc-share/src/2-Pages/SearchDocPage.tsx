import SearchDocF from "../3-Features/SearchDoc/SearchDocF";
import SectionTitle from "../4-Widgets/SectionTitle/SectionTitle";


export default function SearchDocPage() {
  return (
    <div>
      <SectionTitle title={"Поиск на сайте"} />
      <SearchDocF isProfilePage={false} />
    </div>
  );
}
