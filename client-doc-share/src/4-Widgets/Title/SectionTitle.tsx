import "./SectionTitle.css"

type Props = {
    title: string
}

export default function SectionTitle({title}: Props) {
    return (
        <p className="section-title">{title}</p>
    )
}