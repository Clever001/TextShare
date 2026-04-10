import "./ValueInput.css"

type Props = {
    type: "textarea" | "select" | "input" | "checkbox",
    label: string,
    forVal: string,
    formSearchName: string,
    hint?: string | null,
    possibleSelections?: SelectionInfo[],
    defaultValue?: string | null,
}

export type SelectionInfo = {
    htmlValue: string,
    presentValue: string,
}

export default function ValueInput({
    type, label, forVal, formSearchName, hint = null, 
    possibleSelections = [], defaultValue = null
}: Props) {
    return (
        <div className="value-input">
            <p>{label}</p>
            {type == "textarea" &&
                <textarea name="" id=""></textarea>
            }
            {type == "select" &&
                "Select"
            }
            {type == "input" &&
                "Input"
            }
            {type == "checkbox" &&
                "CheckBox"
            }
        </div>
    )
}