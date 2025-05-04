document.addEventListener('DOMContentLoaded', () => {
    //console.log(0)
    const previewSize = 150

    // Open modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]')
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target')
            const modal = document.querySelector(modalTarget)

            if (modal)
                modal.style.display = 'flex';

            if (modalTarget == '#editProjectModal') {
                console.log('EditProjectModalen kommer att öppnas.')
                //document.getElementById('ProjectName').value = "My Text Input";
                console.log(document.getElementById('ProjectName').value)

            }
        })
    })

    // Close modal
    const closeButtons = document.querySelectorAll('[data-close="true"]')
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal')

            if (modal) { 
                modal.style.display = 'none';

                //console.log(document.getElementById('ProjectName').value)

                // Clear formdata and errormessages
                modal.querySelectorAll('form').forEach(form => {
                    form.reset()
                    clearErrorMessages(form)

                    // removes preview of the image
                    const imagePreview = form.querySelector('.image-preview')
                    if (imagePreview)
                        imagePreview.src = ''

                    // removes class 'selected'
                    const imagePreviewer = form.querySelector('.image-previewer')
                    if (imagePreviewer)
                        imagePreviewer.classList.remove('selected')
                })
            }
        })
    })

    // handle dropdowns
    const dropdowns = document.querySelectorAll('[data-type="dropdown"]')
    //console.log(dropdowns)

    document.addEventListener('click', function (event) { 
        let clickedDropdown = null

        dropdowns.forEach(dropdown => {           
            const targetId = dropdown.getAttribute('data-target')
            const targetElement = document.querySelector(targetId)
            //console.log(targetId)

            if (dropdown.contains(event.target)) {
                clickedDropdown = targetElement

                document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                    if (openDropdown !== targetElement) {
                        openDropdown.classList.remove('dropdown-show')
                    }
                })
                targetElement.classList.toggle('dropdown-show')
            }
        })

        if (!clickedDropdown && !event.target.closest('.dropdown')) {
            document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                openDropdown.classList.remove('dropdown-show')
            })
        }
    })

    /* handle Custom Select
    document.querySelector('.form-select').forEach(select => {
        const trigger = select.querySelector('.form-select-trigger')
        const triggerText = trigger.querySelector('.form-select-text')
        const options = select.querySelectorAll('.form-select-option')
        const hiddenInput = select.querySelector('input[type="hidden"]')
        const placeholder = select.dataset.placeholder || "Choose"

        const setValue = (value = "", text = placeholder) => {
            triggerText.textContent = text
            hiddenInput.value = value
            select.classList.toggle('has-placeholder', !value)
        };

        setValue()

        trigger.addEventListener('click', (e) => {
            e.stopPropagation();
            document.querySelectorAll('.form-select-open')
                .forEach(el => el !== select && el.classList.remove('open'))
            select.classList.toggle('open')
        })

        options.forEach(option => {
            option.addEventListener('click', () => {
                setValue(option.dataset.value, option.textContent)
                select.classList.remove('open')
            })
        })

        document.addEventListener('click', e => {
            if (!select.contains(e.target))
                select.classList.remove('open')
        })
    }) */

    
    /* handle image-previewer
    document.querySelectorAll('.image-previewer').forEach(previewer => {
        const fileInput = previewer.querySelector('input[type="file"]')
        const imagePreview = previewer.querySelector('.image-preview')

        previewer.addEventListener('click', () => fileInput.click())

        fileInput.addEventListener('change', ({ target: { files } }) => {
            const file = files[0]
            if (file)
                processImage(file, imagePreview, previewer, previewSize)

        })
    }) */


    // handle submit forms
    const forms = document.querySelectorAll('form')
    forms.forEach(form => { 
        form.addEventListener('submit', async (e) => {
            e.preventDefault()
            
            clearErrorMessages(form)
           
            const formData = new FormData(form)

            try {
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                })
                
                if (res.ok) {
                    const modal = form.closest('.modal')
                   
                    if (modal)
                        modal.style.display = 'none';

                    window.location.reload()                   
                }
                else if (res.status === 400) {
                    const data = await res.json()

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            const input = form.querySelector(`[name="${key}"]`)
                            if (input) {
                                input.classList.add('input-validation-error')
                            }

                            const span = form.querySelector(`[data-valmsg-for="${key}"]`)
                            if (span) {
                                span.innerText = data.errors[key].join('\n')
                                span.classList.add('field-validation-error')
                            }
                        })
                    }
                }
            }
            catch {
                console.log('error submitting the form')
            }

        })
    })

})

function openEditProjectModal(param) {
    console.log("hej från site.js");

    if (param == 'yes')
        console.log("param är yes");

    if (param == 'no')
        console.log("param är no");

    if (param == '')
        console.log("param är blank");

    if (param == 'yes') { 
        const modal = document.querySelector('#editProjectModal')

        if (modal)
            modal.style.display = 'flex';

        console.log("flex has been set");
    }
}

function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error')
    })

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.classList.remove('field-validation-error')
    })
}

async function loadImage(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader()

        reader.onerror = () => reject(new Error("Failed to load file."))
        reader.onload = (e) => {
            const img = new Image()
            img.onerror = () => reject(new Error("Failed to load file."))
            img.onload = () => resolve(img)
            img.src = e.target.result
        }

        reader.readAsDataURL(file)
    })
}

async function processImage(file, imagePreview, previewer, previewSize = 150) {
    try {
        const img = await loadImage(file)
        const canvas = document.createElement('canvas')
        canvas.width = previewSize
        canvas.height = previewSize

        const ctx = canvas.getContext('2d')
        ctx.drawImage(img, 0, 0, previewSize, previewSize)
        imagePreview.src = canvas.toDataURL('image/jpeg')
        previewer.classList.add('selected')
    }
    catch (error) {
        console.error('Failed on image-processing:', error)
    }
}